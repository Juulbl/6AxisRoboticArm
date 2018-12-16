/**
 * @file RemoteControl.cpp
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-16
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#include "RemoteControl.h"

RemoteControl::RemoteControl(Timeline& timeline)
  : timeline(timeline)
{

  //Clear rx buffer.
  this->ClearRxBuffer();

}

RemoteControl::~RemoteControl()
{
}

void RemoteControl::Update()
{

}

void RemoteControl::UpdateSerial() 
{

  //Received character.
  char receivedChar;

  //While serial available.
  while(Serial.available() > 0)
  {

    //Set received character.
    receivedChar = Serial.read();
    
    //Check characters.
    switch(receivedChar)
    {
      case REMOTE_CONTROL_START_SYMBOL:

        //Clear rx buffer.
        this->ClearRxBuffer();

        break;
      case REMOTE_CONTROL_END_SYMBOL:

        //Handle rx buffer as command.
        this->HandleCommand(RemoteControlCommand(this->rxBuffer));

        break;
      default:

        //Add character to rx buffer.
        this->AddCharToRxBuffer(receivedChar);

        break;
    }

  }

}

const int8_t RemoteControl::AddCharToRxBuffer(char c)
{

  //If buffer would overflow, return -1;
  if(this->currentRxBufferIndex >= REMOTE_CONTROL_RX_SIZE) return -1;

  //Add character.
  this->rxBuffer[this->currentRxBufferIndex] = c;

  //Increment current rx buffer index.
  this->currentRxBufferIndex++;

  //Return success.
  return 0;

}

void RemoteControl::ClearRxBuffer()
{

  //Set all bytes in character buffer to 0x00.
  memset(this->rxBuffer, 0x00, REMOTE_CONTROL_RX_SIZE * sizeof(char));

  //Reset current rx buffer index.
  this->currentRxBufferIndex = 0;

}

const int8_t RemoteControl::HandleCommand(RemoteControlCommand&& command)
{
  
  Serial.println(command.GetCommand());
  Serial.println(command.GetParams());

}