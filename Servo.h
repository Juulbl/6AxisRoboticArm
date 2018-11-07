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

#include <stdint.h>


//////////////////////////////////////
// DEFINES
//////////////////////////////////////

#ifndef M_PI
#define M_PI (3.14159265358979323846)
#endif


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

  uint8_t GetServoIndex();
  double GetMinAngle();
  double GetMaxAngle();
  double GetAngle();

private:
  uint8_t servoIndex;
  double minAngle;
  double maxAngle;
  double angle;

};

#endif