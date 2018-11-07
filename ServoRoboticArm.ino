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

#define SERVO_MIN_PULSE (1000)
#define SERVO_MAX_PULSE (2000)
#define SERVO_PULSE_RANGE (SERVO_MAX_PULSE - SERVO_MIN_PULSE)
#define SERVO_PULSE_PER_RADIAN (SERVO_PULSE_RANGE / M_PI)

#define SERVO_DRIVER_FREQUENCY (60)
#define SERVO_DRIVER_PULSE_LENGTH ((1000000 / SERVO_DRIVER_FREQUENCY) / 4096)

#define NUM_OF_SERVOS (6)


//////////////////////////////////////
// FIELDS
//////////////////////////////////////

Adafruit_PWMServoDriver servoDriver = Adafruit_PWMServoDriver();

Servo servos[NUM_OF_SERVOS] = {
  Servo(0, M_PI / 2),
  Servo(1, M_PI / 2),
  Servo(2, M_PI / 2),
  Servo(3, M_PI / 2),
  Servo(4, M_PI / 2),
  Servo(5, M_PI / 2)
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
  servoDriver.setPWMFreq(SERVO_DRIVER_FREQUENCY);

  //Update servos.
  UpdateServos();
}

void loop()
{
	UpdateServos();
}

void UpdateServos()
{
  for(uint8_t i = 0; i < NUM_OF_SERVOS; i++)
  {
    servoDriver.setPWM(servos[i].GetServoIndex(), 0, (SERVO_PULSE_PER_RADIAN * servos[i].GetAngle() + SERVO_MIN_PULSE) / SERVO_DRIVER_PULSE_LENGTH);
  }
}