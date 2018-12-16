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
#include "Sequence.h"


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
   * @brief Set is repeating.
   * 
   * @param isRepeating If the timeline needs to be repeated.
   */
  void SetIsRepeating(const bool isRepeating);

  /**
   * @brief Get the number of servos.
   * 
   * @return const uint8_t Number of servos.
   */
  const uint8_t GetNumberOfServos() const;

  /**
   * @brief If timeline is playing.
   * 
   * @return true If is playing.
   * @return false If is not playing.
   */
  const bool IsPlaying();

  /**
   * @brief If timeline is repeating.
   * 
   * @return true If is repeating.
   * @return false If is not repeating. 
   */
  const bool IsRepeating();

  /**
   * @brief If a sequence is loaded.
   * 
   * @return true If sequence is loaded.
   * @return false If sequence is not loaded.
   */
  const bool IsSequenceLoaded();

  /**
   * @brief Update timeline.
   * 
   * @param deltaTime Deltatime in microseconds.
   */
  void Update(const unsigned long deltaTime);

  /**
   * @brief Load sequences.
   * 
   * @param sequences Sequences to load.
   * 
   * @Pre sequences Needs to have the same length as number of servos.
   */
  void LoadSequences(const Sequence* sequences);

  /**
   * @brief Unloads the loaded sequence.
   * 
   */
  void UnloadSequence();

  /**
   * @brief Play timeline.
   * 
   */
  void Play();

  /**
   * @brief Pauze timeline.
   * 
   */
  void Pause();

  /**
   * @brief Stop timeline.
   * 
   */
  void Stop();

  /**
   * @brief Reset timeline.
   * 
   */
  void Reset();

private:

  const uint8_t numOfServos;    //Keep track of the num of servos in servos.
  Servo* servos;                //Servos the timeline can container.
  unsigned long currentTime;    //Current time(microseconds) the timeline is at.
  bool isPlaying;               //Timeline playing state.
  bool isRepeating;             //If the timeline needs to repeat.
  Sequence* sequences;          //Sequence of keyframe that define the robot's motion. This array needs to be the same length as servos.
  bool isSequenceLoaded;        //If a sequence is loaded.
  unsigned long duration;       //Duration of the timeline.
  
};

#endif