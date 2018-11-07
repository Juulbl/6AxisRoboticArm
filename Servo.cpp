/**
 * @file Servo.cpp
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-11-07
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#include "Servo.h"

Servo::Servo(uint8_t servoIndex)
  : servoIndex(servoIndex), minAngle(0), maxAngle(M_PI)
{
  this->SetAngle(M_PI / 2);
}

Servo::Servo(uint8_t servoIndex, double initialAngle)
  : servoIndex(servoIndex), minAngle(0), maxAngle(M_PI)
{
  this->SetAngle(initialAngle);
}

Servo::Servo(uint8_t servoIndex, double minAngle, double maxAngle)
  : servoIndex(servoIndex), minAngle(minAngle), maxAngle(maxAngle)
{
  this->SetAngle(M_PI / 2);
}

Servo::Servo(uint8_t servoIndex, double minAngle, double maxAngle, double initialAngle)
  : servoIndex(servoIndex), minAngle(minAngle), maxAngle(maxAngle)
{
  this->SetAngle(initialAngle);
}

Servo::~Servo()
{

}

void Servo::SetAngle(double angle)
{
  //Check angles.
  if(angle > maxAngle) angle = maxAngle;
  else if(angle < minAngle) angle = minAngle;

  //Set angle.
  this->angle = angle;
}

uint8_t Servo::GetServoIndex()
{
  return this->servoIndex;
}

double Servo::GetMinAngle()
{
  return this->minAngle;
}

double Servo::GetMaxAngle()
{
  return this->maxAngle;
}

double Servo::GetAngle()
{
  return this->angle;
}