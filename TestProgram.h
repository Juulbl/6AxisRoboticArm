/**
 * @file TestProgram.h
 * @author Juul Bloemers
 * @brief 
 * @version 0.1
 * @date 2018-12-15
 * 
 * @copyright Copyright (c) 2018
 * 
 */
#ifndef TEST_PROGRAM_H
#define TEST_PROGRAM_H


//////////////////////////////////////
// INCLUDES
//////////////////////////////////////

#include "Sequence.h"


//////////////////////////////////////
// FIELDS
//////////////////////////////////////

const static Sequence TestProgram[] = {
  Sequence(
    new SequenceKeyframe[5]{
      {0000000, 0.174533}, 
      {2000000, 2.96706}, 
      {3000000, 2.96706}, 
      {7000000, 0.174533},
      {8000000, 0.174533}
    },
    5
  ),
  Sequence(new SequenceKeyframe[1]{{0000000, M_PI}}, 1),
  Sequence(new SequenceKeyframe[1]{{0000000, 0}}, 1),
  Sequence(new SequenceKeyframe[1]{{0000000, 0}}, 1),
  Sequence(new SequenceKeyframe[1]{{0000000, 0}}, 1),
  Sequence(new SequenceKeyframe[1]{{0000000, 0}}, 1)
};

#endif