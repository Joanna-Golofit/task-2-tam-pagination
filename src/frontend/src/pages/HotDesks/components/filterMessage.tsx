import React from 'react';
import { useTranslation } from 'react-i18next';
import { Message } from 'semantic-ui-react';

type ComponentProps = {
  visible: boolean
}

const FilterMessage: React.FC<ComponentProps> = ({ visible }) => {
  const { t } = useTranslation();

  return (
    <>
      {visible && (
        <Message>
          <Message.Header> {t('hotDesks.dateFilterMsgHeader')}</Message.Header>
          <p>
            {t('hotDesks.dateFilterMsg')}
          </p>
        </Message>
      )}
    </>
  );
};
export default FilterMessage;
