/**
 * @file ServoRoboticArm.ino
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-11-07
 * 
 * @copyright Copyright (c) 2018
 * 
 */
//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include  <stdio.h>

#include "Arduino.h"
#include <Wire.h>
#include <Adafruit_PWMServoDriver.h>

#include "Servo.h"
#include "RemoteControl.h"


//////////////////////////////////////
// DEFINES
//////////////////////////////////////

//Servos.
#define SERVO_MIN_ANGLE   (0.1745329252)
#define SERVO_MAX_ANGLE   (2.9670597284)

#define NUM_OF_SERVOS     (6)


//////////////////////////////////////
// FIELDS
//////////////////////////////////////

//Pwm driver.
Adafruit_PWMServoDriver servoDriver = Adafruit_PWMServoDriver();

//Servos.
Servo servos[NUM_OF_SERVOS] = {
  Servo(0, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, M_PI / 2),
  Servo(1, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, M_PI),
  Servo(2, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0),
  Servo(3, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0),
  Servo(4, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0),
  Servo(5, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0)
};

//Timing.
unsigned long lastMicros;
unsigned long currentMicros;
unsigned long deltaTime;

//Remote control.
RemoteControl remoteControl(servos, NUM_OF_SERVOS);

//////////////////////////////////////
// FUNCTIONS
//////////////////////////////////////

void setup()
{
  //Start serial.
  Serial.begin(9600);

  //Start servo driver.
	servoDriver.begin();
  
  //Set servo driver frequency
  servoDriver.setPWMFreq(60);

  //Wait for pwm driver to initialize.
  delay(10);

  //Update servos.
  UpdateServos();

  //Wait for servos to move to position.
  delay(1000);

}

void loop()
{

  //Update current micros.
  currentMicros = micros();

  //Calculate delta time.
  deltaTime = currentMicros - lastMicros;


  //////////////////////////////////////////


  //Update remote control.
  remoteControl.Update();

  //Update servos.
  UpdateServos();


  //////////////////////////////////////////
  

  //Set last micros to current micros.
  lastMicros = currentMicros;
}

void serialEvent()
{

  //Updated remote control serial.
  remoteControl.UpdateSerial();

}

void UpdateServos()
{

  //Update all servos.
  for(uint8_t i = 0; i < NUM_OF_SERVOS; i++)
  {

    //Set servo pwm pulse.
    servoDriver.setPWM(servos[i].GetServoIndex(), 0, servos[i].GetPulse());

  }

}