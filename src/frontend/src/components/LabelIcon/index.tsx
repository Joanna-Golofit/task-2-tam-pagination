import React from 'react';
import { Icon, Label, SemanticCOLORS, SemanticICONS } from 'semantic-ui-react';

type Props = {
    iconColor: SemanticCOLORS;
    iconName: SemanticICONS;
  };

const LabelIcon: React.FC<Props> = ({ iconColor, iconName, children }) => (
  <Label>
    <Icon color={iconColor} name={iconName} />
    <Label.Detail>
      {children}
    </Label.Detail>
  </Label>
);

export default LabelIcon;
