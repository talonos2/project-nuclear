Re=6378.136
H=900*1000
sidp=Re/(Re+H)
EarthCentralAngle=acosd(Re/(Re+H))
ECAToCal=.5*EarthCentralAngle
n=atand(sidp*sind(ECAToCal)/(1-sidp*cosd(ECAToCal)))
90-(n+ECAToCal)