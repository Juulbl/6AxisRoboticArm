/**
 * @file Sequence.cpp
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-15
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#include "Sequence.h"

Sequence::Sequence(SequenceKeyframe* keyframes, uint16_t numOfFrames)
  : keyframes(keyframes), numOfFrames(numOfFrames), duration(keyframes[numOfFrames - 1].time)
{
}

Sequence::~Sequence()
{

}

const double Sequence::GetAngleAtTime(const unsigned long time) const
{
  //If time is higher than duration.
  if(time >= this->GetDuration()) 
  {

    //Return angle of last keyframe.
    return this->keyframes[this->numOfFrames - 1].angle;

  }

  //Keyframe the transition starts at.
  uint16_t startFrameIndex = 0;

  //While the time of the next keyframe is higher than the requested time, increment start keyframe.
  while(this->keyframes[startFrameIndex + 1].time < time) startFrameIndex++;

  //Calculate angle.
  unsigned long timeDifference = this->keyframes[startFrameIndex + 1].time - this->keyframes[startFrameIndex].time;
  double angleDifference = this->keyframes[startFrameIndex].angle - this->keyframes[startFrameIndex + 1].angle;
  double angleAtTime = this->keyframes[startFrameIndex].angle - ((angleDifference / timeDifference) * (time - this->keyframes[startFrameIndex].time));

  //Return angle.
  return angleAtTime;

}

const uint16_t Sequence::GetNumOfFrames() const
{
  return this->numOfFrames;
}

const unsigned long& Sequence::GetDuration() const
{
  return this->duration;
}