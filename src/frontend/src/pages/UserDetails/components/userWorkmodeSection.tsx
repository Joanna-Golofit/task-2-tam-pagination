import React, { useState } from 'react';
import { CheckboxProps, Radio, Table } from 'semantic-ui-react';
import { useDispatch } from 'react-redux';
import { useTranslation } from 'react-i18next';
import { UserDetailsDto } from '../../../services/user/models';
import SimpleModal from '../../../layouts/components/SimpleModal';
import { updateWorkspaceType } from '../../../store/userDetails/actions';
import { setLoadingAction } from '../../../store/global/actions';

interface Props {
  userDetails: UserDetailsDto;
}

const UserWorkmodeSection:React.FC<Props> = ({ userDetails }) => {
  const { t } = useTranslation();
  const dispatch = useDispatch();
  const { id, workspaceType, locations } = userDetails;
  const [isRemoteModeConfirmationDialogOpen, setIsRemoteModeConfirmationDialogOpen] = useState(false);

  const workspaceChangeRadioBtnHandler = ((_: React.FormEvent<HTMLInputElement>, data: CheckboxProps) => {
    if (locations.length !== 0 && data.value === 1) {
      setIsRemoteModeConfirmationDialogOpen(true);
    } else {
      dispatch(setLoadingAction(true));
      dispatch(updateWorkspaceType(
        [{ employeeId: id, workspaceType: data.value as number }],
      ));
    }
  });

  const remoteModeConfirmationDialogYesBtnClickHandler = () => {
    setIsRemoteModeConfirmationDialogOpen(false);
    dispatch(setLoadingAction(true));
    dispatch(updateWorkspaceType(
      [{ employeeId: id, workspaceType: 1 }],
    ));
  };

  return (
    <>
      <Table color="orange" unstackable>
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell textAlign="center">
              {t('projectDetails.officeWork')}
            </Table.HeaderCell>
            <Table.HeaderCell textAlign="center">
              {t('projectDetails.hybridWork')}
            </Table.HeaderCell>
            <Table.HeaderCell textAlign="center">
              {t('projectDetails.remoteWork')}
            </Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          <Table.Row key={id}>
            <Table.Cell textAlign="center">
              <Radio
                checked={workspaceType === 0}
                name={id}
                value={0}
                onChange={workspaceChangeRadioBtnHandler}
              />
            </Table.Cell>
            <Table.Cell textAlign="center">
              <Radio
                checked={workspaceType === 2}
                name={id}
                value={2}
                onChange={workspaceChangeRadioBtnHandler}
              />
            </Table.Cell>
            <Table.Cell textAlign="center">
              <Radio
                checked={workspaceType === 1}
                name={id}
                value={1}
                onChange={workspaceChangeRadioBtnHandler}
              />
            </Table.Cell>
          </Table.Row>
        </Table.Body>
      </Table>
      <SimpleModal
        isOpen={isRemoteModeConfirmationDialogOpen}
        text={t('projectDetails.setRemoteMessage')}
        closeFunction={() => setIsRemoteModeConfirmationDialogOpen(false)}
        yesOkFunction={remoteModeConfirmationDialogYesBtnClickHandler}
        isOkBtnOnly={false}
      />
    </>
  );
};

export default UserWorkmodeSection;
