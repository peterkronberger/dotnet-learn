import React from 'react';
import Choice from './Choice';

export default function SingleChoice(props) {
  return <Choice {...props} multi={false} />;
}
