/**
 * @file Timeline.cpp
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-15
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#include "Timeline.h"

Timeline::Timeline(Servo* servos, uint8_t numOfServos) 
  : servos(servos), numOfServos(numOfServos)
{

}

Timeline::~Timeline() 
{

}

void Timeline::Update(const unsigned long deltaTime)
{
  //Update timeline time.
  this->currentTime += deltaTime;

  Serial.println(this->currentTime);
}