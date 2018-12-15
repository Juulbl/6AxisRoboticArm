/**
 * @file Timeline.h
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-15
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#ifndef TIMELINE_H
#define TIMELINE_H


//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include <stdio.h>

#include "Arduino.h"

#include "Servo.h"


//////////////////////////////////////
// CLASSES
//////////////////////////////////////

class Timeline {

public:

  /**
   * @brief Construct a new Timeline object.
   * 
   * @param servos Servos to control.
   * @param numOfServos Number of servos in servos.
   */
  Timeline(Servo* servos, uint8_t numOfServos);

  ~Timeline();

  /**
   * @brief Update timeline.
   * 
   * @param deltaTime Deltatime in microseconds.
   */
  void Update(const unsigned long deltaTime);

private:

  Servo* servos;              //Servos the timeline can container.
  uint8_t numOfServos;        //Keep track of the num of servos in servos.
  unsigned long currentTime;  //Current time(microseconds) the timeline is at.

};

#endif