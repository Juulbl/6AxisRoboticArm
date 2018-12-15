/**
 * @file Servo.h
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-11-07
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#ifndef SERVO_H
#define SERVO_H


//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include "Arduino.h"

#include <stdint.h>


//////////////////////////////////////
// DEFINES
//////////////////////////////////////

//Servo
#define SERVO_MIN_PULSE (157)
#define SERVO_MAX_PULSE (543)

//Degrees/Radians
#define DEGREES_PER_RADIAN (180.0 / M_PI)

//M_PI
#ifndef M_PI
#define M_PI (3.14159265358979323846)
#endif


//////////////////////////////////////
// MACROS
//////////////////////////////////////

#define SERVO_ANGLE_TO_PULSE(SERVO_ANGLE) (map(SERVO_ANGLE * DEGREES_PER_RADIAN, 0, 180, SERVO_MIN_PULSE, SERVO_MAX_PULSE))


//////////////////////////////////////
// CLASSES
//////////////////////////////////////

class Servo {

public:
  /**
   * @brief Construct a new Servo object
   * 
   * @param servoIndex Index the servo has on the PWM driver.
   */
  Servo(uint8_t servoIndex);

  /**
   * @brief Construct a new Servo object
   * 
   * @param servoIndex Index the servo has on the PWM driver.
   * @param initialAngle Value the servo is set to on initialization.
   */
  Servo(uint8_t servoIndex, double initialAngle);

  /**
   * @brief Construct a new Servo object
   * 
   * @param servoIndex Index the servo has on the PWM driver.
   * @param minAngle Minimal angle the servo can be at.
   * @param maxAngle Maximum angle the servo can be at.
   */
  Servo(uint8_t servoIndex, double minAngle, double maxAngle);

  /**
   * @brief Construct a new Servo object
   * 
   * @param servoIndex Index the servo has on the PWM driver.
   * @param minAngle Minimal angle the servo can be at.
   * @param maxAngle Maximum angle the servo can be at.
   * @param initialAngle Value the servo is set to on initialization.
   */
  Servo(uint8_t servoIndex, double minAngle, double maxAngle, double initialAngle);

  ~Servo();

  /**
   * @brief Set servo angle.
   * 
   * @param angle Angle in radians.
   */
  void SetAngle(double angle);

  /**
   * @brief Get Index the servo has on the PWM driver.
   * 
   * @return const uint8_t& Servo index.
   */
  const uint8_t& GetServoIndex() const;

  /**
   * @brief Get the servo its minimal angle.
   * 
   * @return const double& Minimal angle in radians.
   */
  const double& GetMinAngle() const;

  /**
   * @brief Get the servo its maximum angle.
   * 
   * @return const double& Maximum angle in radians.
   */
  const double& GetMaxAngle() const;

  /**
   * @brief Get the servo its angle.
   * 
   * @return const double& Angle in radians.
   */
  const double& GetAngle() const;

  /**
   * @brief Get the PWM pulse needed to get the desired angle.
   * 
   * @return const uint16_t& PWM pulse.
   */
  const uint16_t& GetPulse() const;

private:
  const uint8_t servoIndex;
  const double minAngle;
  const double maxAngle;
  double angle;
  uint16_t pulse;

};

#endif