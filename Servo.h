/**
 * @file Servo.h
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-11-07
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#ifndef SERVO_H
#define SERVO_H


//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include "Arduino.h"

#include <stdint.h>


//////////////////////////////////////
// DEFINES
//////////////////////////////////////

//Servo
#define SERVO_MIN_PULSE (157)
#define SERVO_MAX_PULSE (543)

//Degrees/Radians
#define DEGREES_PER_RADIAN (180.0 / M_PI)

//M_PI
#ifndef M_PI
#define M_PI (3.14159265358979323846)
#endif


//////////////////////////////////////
// MACROS
//////////////////////////////////////

#define SERVO_ANGLE_TO_PULSE(SERVO_ANGLE) (map(SERVO_ANGLE * DEGREES_PER_RADIAN, 0, 180, SERVO_MIN_PULSE, SERVO_MAX_PULSE))


//////////////////////////////////////
// CLASSES
//////////////////////////////////////

class Servo {

public:
  Servo(uint8_t servoIndex);
  Servo(uint8_t servoIndex, double initialAngle);
  Servo(uint8_t servoIndex, double minAngle, double maxAngle);
  Servo(uint8_t servoIndex, double minAngle, double maxAngle, double initialAngle);
  ~Servo();

  void SetAngle(double angle);

  const uint8_t& GetServoIndex() const;
  const double& GetMinAngle() const;
  const double& GetMaxAngle() const;
  const double& GetAngle() const;
  const uint16_t& GetPulse() const;

private:
  uint8_t servoIndex;
  double minAngle;
  double maxAngle;
  double angle;
  uint16_t pulse;

};

#endif