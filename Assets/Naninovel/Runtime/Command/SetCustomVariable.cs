﻿// Copyright 2017-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityCommon;
using UnityEngine;

namespace Naninovel.Commands
{
    /// <summary>
    /// Assigns result of a [script expression](/guide/script-expressions.md) to a [custom variable](/guide/custom-variables.md).
    /// </summary>
    /// <remarks>
    /// Variable name should be alphanumeric (latin characters only) and can contain underscores, eg: `name`, `Char1Score`, `my_score`;
    /// the names are case-insensitive, eg: `myscore` is equal to `MyScore`. If a variable with the provided name doesn't exist, it will be automatically created.
    /// <br/><br/>
    /// It's possible to define multiple set expressions in one line by separating them with `;`. The expressions will be executed in sequence by the order of declaratation.
    /// <br/><br/>
    /// Custom variables are stored in **local scope** by default. This means, that if you assign some variable in the course of gameplay 
    /// and player starts a new game or loads another saved game slot, where that variable wasn't assigned — the value will be lost. 
    /// If you wish to store the variable in **global scope** instead, prepend `G_` or `g_` to its name, eg: `G_FinishedMainRoute` or `g_total_score`.
    /// <br/><br/>
    /// You can get and set custom variables in C# scripts via `CustomVariableManager` [engine service](/guide/engine-services.md).
    /// </remarks>
    /// <example>
    /// ; Assign `foo` variable a `bar` string value
    /// @set foo="bar"
    /// 
    /// ; Assign `foo` variable a 1 number value
    /// @set foo=1
    /// 
    /// ; Assign `foo` variable a `true` boolean value
    /// @set foo=true
    /// 
    /// ; If `foo` is a number, add 0.5 to its value
    /// @set foo=foo+0.5
    /// 
    /// ; If `angle` is a number, assign its cosine to `result` variable
    /// @set result=Cos(angle)
    /// 
    /// ; Get a random integer between -100 and 100, then raise to power of 4 and assign to `result` variable
    /// @set "result = Pow(Random(-100, 100), 4)"
    /// 
    /// ; Assign `foo` variable value of the `bar` variable, which is `Hello World!`.
    /// ; Notice, that `bar` variable should actually exist, otherwise `bar` plain text value will be assigned instead.
    /// @set bar="Hello World!"
    /// @set foo=bar
    /// 
    /// ; Defining multiple set expressions in one line (the result will be the same as above)
    /// @set bar="Hello World!";foo=bar
    /// 
    /// ; It's possible to inject variables to naninovel script command parameters
    /// @set scale=0
    /// # EnlargeLoop
    /// @char Misaki.Default scale:{scale}
    /// @set scale=scale+0.1
    /// @goto .EnlargeLoop if:scale&lt;1
    /// 
    /// ; ..and generic text lines
    /// @set name="Dr. Stein";drink="Dr. Pepper"
    /// {name}: My favourite drink is {drink}!
    /// 
    /// ; When using double quotes inside the expression itself, don't forget to double-escape them
    /// @set remark="Saying \\"Stop the car\\" was a mistake."
    /// </example>
    [CommandAlias("set")]
    public class SetCustomVariable : Command
    {
        /// <summary>
        /// Set expression. 
        /// <br/><br/>
        /// The expression should be in the following format: `VariableName=ExpressionBody`, where `VariableName` is the name of the custom 
        /// variable to assign and `ExpressionBody` is a [script expression](/guide/script-expressions.md), the result of which should be assigned to the variable.
        /// </summary>
        [CommandParameter(alias: NamelessParameterAlias)]
        public string Expression { get => GetDynamicParameter<string>(null); set => SetDynamicParameter(value); }

        private const string assignmentLiteral = "=";
        private const string separatorLiteral = ";";

        private List<KeyValuePair<string, string>> undoData = new List<KeyValuePair<string, string>>();

        public override async Task ExecuteAsync ()
        {
            undoData.Clear();

            var variableManager = Engine.GetService<CustomVariableManager>();
            var saveStatePending = false;
            var expressions = Expression.Split(separatorLiteral[0]);
            foreach (var expression in expressions)
            {
                if (string.IsNullOrEmpty(expression)) continue;

                var variableName = expression.GetBefore(assignmentLiteral)?.TrimFull();
                var expressionBody = expression.GetAfterFirst(assignmentLiteral)?.TrimFull();
                if (string.IsNullOrWhiteSpace(variableName) || string.IsNullOrWhiteSpace(expressionBody))
                {
                    LogErrorMsg("Failed to extract variable name and expression body. Make sure the expression starts with a variable name followed by assignment operator `=`.");
                    continue;
                }

                var result = ExpressionEvaluator.Evaluate<string>(expressionBody, LogErrorMsg);
                if (result is null) continue;

                undoData.Add(new KeyValuePair<string, string>(variableName, variableManager.GetVariableValue(variableName) ?? string.Empty));

                variableManager.SetVariableValue(variableName, result);
                saveStatePending = saveStatePending || variableManager.IsGlobalVariable(variableName);
            }

            if (saveStatePending)
                await Engine.GetService<StateManager>().SaveGlobalStateAsync();
        }

        public override async Task UndoAsync ()
        {
            if (undoData.Count == 0) return;

            var variableManager = Engine.GetService<CustomVariableManager>();
            var saveStatePending = false;

            foreach (var kv in undoData)
            {
                variableManager.SetVariableValue(kv.Key, kv.Value);
                saveStatePending = saveStatePending || variableManager.IsGlobalVariable(kv.Key);
            }

            if (saveStatePending)
                await Engine.GetService<StateManager>().SaveGlobalStateAsync();

            undoData.Clear();
            return;
        }

        private void LogErrorMsg (string desc = null) => Debug.LogError($"Failed to evaluate set expression `{Expression}` at `{ScriptName}` script at line #{LineNumber}. {desc ?? string.Empty}");
    }
}
