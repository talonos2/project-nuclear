﻿// Copyright 2017-2019 Elringus (Artyom Sovetnikov). All Rights Reserved.

using UnityCommon;
using UnityEngine;

namespace Naninovel
{
    /// <summary>
    /// Allows setting <see cref="ManagedText"/> by associating <see cref="GameObject"/> name and defined managed text fields.
    /// Underscore ("_") chars and casing in the defined field names are ignored.
    /// </summary>
    public abstract class ManagedTextSetter : MonoBehaviour
    {
        protected abstract void SetManagedTextValue (string value);

        private void HandleLocaleChanged (string locale)
        {
            var type = GetType();
            var managedText = ManagedTextUtils.GetManagedTextFromType(type);

            foreach (var text in managedText)
            {
                var fieldName = text.FieldId.GetAfter(".").Replace("_", string.Empty);
                if (gameObject.name.EqualsFastIgnoreCase(fieldName)) { SetManagedTextValue(text.FieldValue); break; }
            }
        }

        private void OnEnable ()
        {
            var localeManager = Engine.GetService<LocalizationManager>();
            if (localeManager != null)
                localeManager.OnLocaleChanged += HandleLocaleChanged;

            HandleLocaleChanged(null);
        }

        private void OnDisable ()
        {
            var localeManager = Engine.GetService<LocalizationManager>();
            if (localeManager != null)
                localeManager.OnLocaleChanged -= HandleLocaleChanged;
        }
    }
}
