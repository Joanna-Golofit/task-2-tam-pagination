import React from 'react';
import { useTranslation } from 'react-i18next';
import { Message } from 'semantic-ui-react';

type ComponentProps = {
  visible: boolean
}

const OutlookBookingMessage: React.FC<ComponentProps> = ({ visible }) => {
  const { t } = useTranslation();

  return (
    <>
      {visible && (
        <Message negative>
          <Message.Header> {t('common.attention')}</Message.Header>
          <p>
            {t('rooms.bookingRoomViaOutlook')}
          </p>
        </Message>
      )}
    </>
  );
};
export default OutlookBookingMessage;
