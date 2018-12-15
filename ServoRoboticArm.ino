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


//////////////////////////////////////
// DEFINES
//////////////////////////////////////

//Servos.
#define SERVO_MIN_ANGLE (0.1745329252)
#define SERVO_MAX_ANGLE (2.9670597284)

#define NUM_OF_SERVOS (6)


//////////////////////////////////////
// FIELDS
//////////////////////////////////////

Adafruit_PWMServoDriver servoDriver = Adafruit_PWMServoDriver();

Servo servos[NUM_OF_SERVOS] = {
  Servo(0, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, M_PI / 2),
  Servo(1, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, M_PI),
  Servo(2, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0),
  Servo(3, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0),
  Servo(4, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0),
  Servo(5, SERVO_MIN_ANGLE, SERVO_MAX_ANGLE, 0)
};


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

  delay(10);

  //Update servos.
  UpdateServos();
}

void loop()
{
  //Update servos.
	UpdateServos();

  delay(1000);

  servos[0].SetAngle(0);
  servos[3].SetAngle(M_PI);
  UpdateServos();

  delay(1000);

  servos[1].SetAngle(1.5);
  UpdateServos();

  delay(1000);

  servos[3].SetAngle(M_PI);
  UpdateServos();

  delay(1000);

  servos[1].SetAngle(M_PI);
  UpdateServos();

  delay(1000);

  servos[3].SetAngle(0);
  UpdateServos();

  delay(1000);

  servos[0].SetAngle(M_PI);
  servos[3].SetAngle(M_PI);
  UpdateServos();

  delay(1000);

  servos[1].SetAngle(1.5);
  UpdateServos();

  delay(1000);

  servos[3].SetAngle(M_PI);
  UpdateServos();

  delay(1000);

  servos[1].SetAngle(M_PI);
  UpdateServos();

  delay(1000);

  servos[3].SetAngle(0);
  UpdateServos();

  delay(1000);

  servos[0].SetAngle(M_PI / 2);
  servos[3].SetAngle(M_PI / 2);
  UpdateServos();

  delay(1000);
}

void UpdateServos()
{
  for(uint8_t i = 0; i < NUM_OF_SERVOS; i++)
  {
    servoDriver.setPWM(servos[i].GetServoIndex(), 0, servos[i].GetPulse());
  }
}