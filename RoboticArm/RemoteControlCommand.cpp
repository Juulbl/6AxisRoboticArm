/**
 * @file RemoteControlCommand.cpp
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-16
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#include "RemoteControlCommand.h"

RemoteControlCommand::RemoteControlCommand(char* str)
{

  //String to command.
  this->StrToCommand(str);

}

RemoteControlCommand::~RemoteControlCommand()
{
}

const char* RemoteControlCommand::GetCommand() const
{
  return this->command;
}

const char* RemoteControlCommand::GetParams() const
{
  return this->params;
}

const int8_t RemoteControlCommand::StrToCommand(char* str)
{
  //Length of str.
  uint16_t strLength = strlen(str);

  //Clear command and params.
  this->ClearCommand();
  this->ClearParams();

  //If length of string is higher than size of params + size of command, return -1;
  if(strLength > REMOTE_CONTROL_COMMAND_COMMAND_SIZE + REMOTE_CONTROL_COMMAND_PARAMS_SIZE) return -1;

  //Split index.
  uint16_t splitIndex = 0;

  //Loop through each character.
  for(uint16_t i = 0; i < strlen(str); i++) 
  {

    //Check if entering params.
    if(str[i] == REMOTE_CONTROL_COMMAND_SPLIT_SYMBOL)
    {

      //Set split index;
      splitIndex = i;

      //Break.
      break;

    }
  }

  //If split index is 0.
  if(splitIndex == 0)
  {

    //Set split location to str length.
    splitIndex = strLength;

  }
  else
  {

    //Get params length.
    uint16_t paramsLength = strLength - splitIndex - 1;

    //If params length is bigger than it's size, return -1.
    if(paramsLength >= REMOTE_CONTROL_COMMAND_PARAMS_SIZE) return -1;

    //If params length is heigher than 0.
    if(paramsLength > 0)
    {

      //Set params.
      strncpy(this->params, &str[splitIndex + 1], paramsLength);
      this->params[paramsLength] = '\0';

    }

  }
  
  //if command length if bigger than it's size or if split index equals 0, return -1.
  if(splitIndex == 0 || splitIndex > REMOTE_CONTROL_COMMAND_COMMAND_SIZE) return -1;

  //Set command.
  strncpy(this->command, str, splitIndex);
  this->command[splitIndex] = '\0';

  //Return success.
  return 0;

}

const void RemoteControlCommand::ClearCommand()
{

  //Set character at index 0 to null terminator.
  this->command[0] = '\0';

}

const void RemoteControlCommand::ClearParams()
{

  //Set character at index 0 to null terminator.
  this->params[0] = '\0';

}