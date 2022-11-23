import React from 'react';
import i18next from 'i18next';
import { SemanticDatepickerProps } from 'react-semantic-ui-datepickers/dist/types';
import SemanticDatepicker from 'react-semantic-ui-datepickers';

function Datepicker(props: SemanticDatepickerProps) {
  return (
    <SemanticDatepicker
      {...props}
      locale={i18next.language}
      firstDayOfWeek={i18next.language.includes('pl') ? 1 : 0}
    />
  );
}

Datepicker.defaultProps = SemanticDatepicker.defaultProps;

export default Datepicker;
