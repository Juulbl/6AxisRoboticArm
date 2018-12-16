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

  //Unload sequence.
  this->UnloadSequence();

  //Set servos null.
  this->servos = nullptr;

}

void Timeline::SetIsRepeating(const bool isRepeating)
{
  this->isRepeating = isRepeating;
}

const uint8_t Timeline::GetNumberOfServos() const
{
  return this->numOfServos;
}

const bool Timeline::IsPlaying()
{
  return this->isPlaying;
}

const bool Timeline::IsRepeating()
{
  return this->isRepeating;
}

const bool Timeline::IsSequenceLoaded()
{
  return this->isSequenceLoaded;
}

void Timeline::Update(const unsigned long deltaTime)
{
  //If not playing, return.
  if(!this->isPlaying) return;

  //Update timeline time.
  this->currentTime += deltaTime;

  //If current time is higher than duration.
  if(this->currentTime > this->duration)
  {

    //Stop timeline.
    this->Stop();

    //If is repeating, start timeline.
    if(this->IsRepeating()) this->Play();

    //Return.
    return;

  }

  //Update servo angles.
  for(uint8_t i = 0; i < this->numOfServos; i++) 
  {

    //Set servo angle.
    this->servos[i].SetAngle(this->sequences[i].GetAngleAtTime(this->currentTime));

  }

}

void Timeline::LoadSequences(const Sequence* sequences)
{
  
  //Unload sequence.
  this->UnloadSequence();

  //Copy sequences.
  size_t sequencesSize = sizeof(*sequences) * this->numOfServos;
  this->sequences = (Sequence*)malloc(sequencesSize);
  memcpy(this->sequences, sequences, sequencesSize);

  //Set timeline duration.
  for(uint8_t i = 0; i < this->numOfServos; i++) 
  {

    //If duration of sequence is longer than timeline duration, set timeline duration to sequence duration.
    if(this->duration < this->sequences[i].GetDuration()) 
    {

      this->duration = this->sequences[i].GetDuration();

    }

  }

  //Set is sequence loaded to true.
  this->isSequenceLoaded = true;

}

void Timeline::UnloadSequence()
{

  //Set is sequence loaded to false.
  this->isSequenceLoaded = false;

  //Set duration to 0.
  this->duration = 0;

  //Delete sequences.
  delete[] this->sequences;

}

void Timeline::Play() 
{

  //If no sequence is loaded, return.
  if(!this->IsSequenceLoaded()) return;

  //Set is playing to true.
  this->isPlaying = true;

}

void Timeline::Pause()
{

  //Set is playing to false.
  this->isPlaying = false;

}

void Timeline::Stop()
{

  //Set is playing to false.
  this->isPlaying = false;

  //Reset timeline.
  this->Reset();

}

void Timeline::Reset()
{

  //Set current time to 0.
  this->currentTime = 0;

}