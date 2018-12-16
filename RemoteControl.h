/**
 * @file RemoteControl.h
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-16
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#ifndef REMOTE_CONTROL_H
#define REMOTE_CONTROL_H


//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include <stdint.h>

#include "Arduino.h"

#include "Servo.h"
#include "RemoteControlCommand.h"


//////////////////////////////////////
// DEFINES
//////////////////////////////////////

#define REMOTE_CONTROL_START_SYMBOL   ('#')
#define REMOTE_CONTROL_END_SYMBOL     ('%')

#define REMOTE_CONTROL_RX_SIZE        (128)


//////////////////////////////////////
// CLASSES
//////////////////////////////////////

class RemoteControl {

public:

  /**
   * @brief Construct a new Remote Control object
   * 
   * @param servos Servo array.
   * 
   * @pre Serial needs to be initialized.
   */
  RemoteControl(Servo* servos, uint8_t numOfServos);

  ~RemoteControl();

  /**
   * @brief Update remote control.
   * 
   */
  void Update();

  /**
   * @brief Update serial communication.
   * 
   */
  void UpdateSerial();

  /**
   * @brief Adds a character to the rx buffer.
   * 
   * @param c Character to add.
   * 
   * @return int8_t Positive if success.
   */
  const int8_t AddCharToRxBuffer(char c);

  /**
   * @brief Clears the rx buffer.
   * 
   */
  void ClearRxBuffer();

  /**
   * @brief Handles a given command.
   * 
   * @return int8_t Positive if success.
   */
  const int8_t HandleCommand(RemoteControlCommand&& command);

private:

  const uint8_t numOfServos;              //Number of servos.
  Servo* servos;                          //Servo array.
  char rxBuffer[REMOTE_CONTROL_RX_SIZE];  //Buffer for receiving data.
  uint16_t currentRxBufferIndex;          //The current index in the rx buffer.

};

#endif