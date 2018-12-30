![](https://raw.githubusercontent.com/Juulbl/6AxisRoboticArm/master/media/images/RobotArmStage3-2.jpeg)
<br/>
This project exists of two different programs. A C++ program to control the robot and a C# program to easily create and run programs / sequences for the robot. All programs where written on a Linux machine.
<br/>
<br/>
## Movement demos
<br/>
### 6 axis
![](https://raw.githubusercontent.com/Juulbl/6AxisRoboticArm/master/media/images/RobotArm6AxisDemo.gif)
<br/>
### Not a robot
![](https://raw.githubusercontent.com/Juulbl/6AxisRoboticArm/master/media/images/RobotArmNotARobot.gif)
<br/>
<br/>
## Hardware
<br/>
### Motors
<br/>
Servos drive all the joints of the robot. Because all joints carry a different weight, three different kinds of servos are used.
<br/>
Servos:
- MG996R:<br/>This servo is used at joints two and three, these joints bare the most weight and therefor need high torque servos.
- S3003:<br/>This servo is used at joints one and four, these joint don't carry a lot of weight so they don't need high torque servos.
- SG90:<br/>This servo is used in the last two joints, these joint carry the least amount of weight and needed to be small.
<br/>
![](https://raw.githubusercontent.com/Juulbl/6AxisRoboticArm/master/media/images/RobotArmServos.jpeg)
<br/>
### Drivers
<br/>
All servos require their own PWM signals. It would take a lot of CPU power if the micro controller needed to handle this by itself. Because of this an external PWM driver is used, in this case the [Adafruit PCA9685](https://www.adafruit.com/product/815). This driver can handle up to 16 servos at 6 Volts. This specific driver was chosen because it could handle enough servos and uses an I2c interface to communicate with the micro controller.
<br/>
### Micro controllers
<br/>
The robot was build to interpret incoming(UART) data and convert it to the correct servo angle which it than send to the PWM/Servo driver. This made it also possible to create a front-end which acts like a time line and makes it posible to update the robot's position live when programming its movement.
<br/>
<br/>
A micro controller was chosen which could "easily" use UART as well as I2c communication. Because a lot of the intensive calculations are done at the computer running the front-end the micro controller didn't need to have a really fast CPU, therefor the [Arduino Uno](https://store.arduino.cc/arduino-uno-rev3) was chosen.
<br/>
## Robot design/model
<br/>
![](https://raw.githubusercontent.com/Juulbl/6AxisRoboticArm/master/media/images/RobotArmDesign.png)
<br/>
The design of the robot was created using [Fusion 360](https://www.autodesk.com/products/fusion-360/overview). The files can be found in the [Fusion 360 directory](https://github.com/Juulbl/6AxisRoboticArm/tree/master/Fusion360).
<br/>
## C++ / Arduino software
<br/>
This project was build using [Visual studio code](https://code.visualstudio.com/). Although this program was written to run on an Arduino, tried was to use Arduino features and functions as little as possible. This was done so the software could easily be changed to run on another C++ compatible micro controller.
<br/>
<br/>
Doxygen was used for commenting, in this way documentation can be easily generated. The documentation with all classes, methods, etc. can be found in the [Documentations directory](https://github.com/Juulbl/6AxisRoboticArm/blob/master/Documentations/RoboticArm).
<br/><br/>[Project directory](https://github.com/Juulbl/6AxisRoboticArm/tree/master/RoboticArm).
<br/>
## C# / GTK software
<br/>
![](https://raw.githubusercontent.com/Juulbl/6AxisRoboticArm/master/media/images/RobotInterfaceFrames.png)
<br/>
This project was build using [Mono develop](https://www.monodevelop.com/).
<br/>
For commanting sandcastle was used.
<br/>
<br/>
[Project directory.](https://github.com/Juulbl/6AxisRoboticArm/tree/master/RobotInterface)