/**
 * @file RemoteControlCommand.h
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-16
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#ifndef REMOTE_CONTROL_COMMAND_H
#define REMOTE_CONTROL_COMMAND_H


//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include "Arduino.h"


//////////////////////////////////////
// DEFINES
//////////////////////////////////////

#define REMOTE_CONTROL_COMMAND_COMMAND_SIZE   (64)
#define REMOTE_CONTROL_COMMAND_PARAMS_SIZE    (64)

#define REMOTE_CONTROL_COMMAND_SPLIT_SYMBOL   (':')


//////////////////////////////////////
// CLASSES
//////////////////////////////////////

class RemoteControlCommand{

public:

  /**
   * @brief Construct a new Remote Control Command object.
   * 
   * @param str Character array to command with parameters.
   */
  RemoteControlCommand(char* str);

  ~RemoteControlCommand();

  /**
   * @brief Get command.
   * 
   * @return const char* 
   */
  const char* GetCommand() const;

  /**
   * @brief Get parameters.
   * 
   * @return const char* 
   */
  const char* GetParams() const;

private:

  char command[REMOTE_CONTROL_COMMAND_COMMAND_SIZE];    //Command buffer.
  char params[REMOTE_CONTROL_COMMAND_PARAMS_SIZE];      //Params buffer.

  /**
   * @brief Convert string to command.
   * 
   * @param str Character array.
   * @return const int8_t Positive if success.
   */
  const int8_t StrToCommand(char* str);

  /**
   * @brief Clears command.
   * 
   */
  const void ClearCommand();

  /**
   * @brief Clears params.
   * 
   */
  const void ClearParams();

};

#endif