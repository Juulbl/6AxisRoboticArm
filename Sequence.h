/**
 * @file Sequence.h
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-15
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#ifndef SEQUENCE_H
#define SEQUENCE_H


//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include <stdint.h>

#include "Arduino.h"

#include "SequenceKeyframe.h"


//////////////////////////////////////
// CLASSES
//////////////////////////////////////

class Sequence {

public:

  Sequence(SequenceKeyframe* keyframes, uint16_t numOfFrames);

  ~Sequence();

  /**
   * @brief Get the angle at the given position.
   * 
   * @param time Time as microseconds.
   * @return const double Angle at time.
   */
  const double GetAngleAtTime(const unsigned long time) const;

  /**
   * @brief Get number of frames.
   * 
   * @return const uint16_t Number of frames.
   */
  const uint16_t GetNumOfFrames() const;

  /**
   * @brief Get sequence duration.
   * 
   * @return const unsigned& GetDuration Sequence duration.
   */
  const unsigned long& GetDuration() const;

private:

  const SequenceKeyframe* keyframes;    //Keyframes define the robot's motion.
  const uint16_t numOfFrames;           //Number of keyframes.
  const unsigned long duration;         //Duration of the sequence.

};

#endif