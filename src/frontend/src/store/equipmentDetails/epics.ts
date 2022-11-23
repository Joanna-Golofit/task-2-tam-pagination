import { combineEpics, Epic, ofType } from 'redux-observable';
import { of } from 'rxjs';
import { catchError, filter, map, switchMap } from 'rxjs/operators';
import i18n from '../../i18n';
import { ErrorCodes } from '../../services/common/models';
import editEquipmentApiService from '../../services/equipments/editEquipmentApiService';
import getEquipmentApiService from '../../services/equipments/getEquipmentApiService';
import removeEquipmentApiService from '../../services/equipments/removeEquipmentApiService';
import reserveEquipmentApiService from '../../services/equipments/reserveEquipmentApiService';
import { setLoadingAction, showErrorCodeInNotifyAction, showNotifyAction } from '../global/actions';
import { editEquipmentSuccessAction, fetchEquipmentDetailsAction, fetchEquipmentDetailsSuccessAction, reserveEquipmentDataSuccessAction } from './actions';
import { EDIT_EQUIPMENT_DATA, FETCH_EQUIPMENT_DETAILS, REMOVE_EQUIPMENT, RESERVE_EQUIPMENT_DATA } from './types';

const handleApiErrorAndStopLoader = () =>
  catchError((error: any) => {
    if (error.name || error.name === 'AjaxError') {
      const errorMessage = error.response.translationKey ? i18n.t(error.response.translationKey) : error.response.message;
      return of(showNotifyAction(errorMessage, true, ' '), setLoadingAction(false));
    }
    return of(showErrorCodeInNotifyAction(ErrorCodes.UnknownError), setLoadingAction(false));
  });

const editEquipmentEpic: Epic = (action$) => action$
  .ofType(EDIT_EQUIPMENT_DATA)
  .pipe(
    switchMap(({ equipment }) => editEquipmentApiService(equipment)
      .pipe(
        switchMap(() => of(
          fetchEquipmentDetailsAction(equipment.equipmentId),
          editEquipmentSuccessAction(),
          showNotifyAction(`${i18n.t('equipmentDetails.editSuccessNotification')}`),
        )),
        handleApiErrorAndStopLoader(),
      )),
  );

const reserveEquipmentEpic: Epic = (action$) => action$
  .ofType(RESERVE_EQUIPMENT_DATA)
  .pipe(
    switchMap(({ reservation }) => reserveEquipmentApiService(reservation)
      .pipe(
        switchMap(() => of(
          fetchEquipmentDetailsAction(reservation.equipmentId),
          reserveEquipmentDataSuccessAction(),
          showNotifyAction(`${i18n.t('equipmentDetails.editSuccessNotification')}`),
        )),
        handleApiErrorAndStopLoader(),
      )),
  );

const getEquipmentDetailsEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === FETCH_EQUIPMENT_DETAILS),
  switchMap((action) => getEquipmentApiService(action.id).pipe(
    switchMap((equipment) => of(fetchEquipmentDetailsSuccessAction(equipment), setLoadingAction(false))),
    handleApiErrorAndStopLoader(),
  )),
);

const removeEquipmentEpic: Epic = (action$) => action$.pipe(
  ofType(REMOVE_EQUIPMENT),
  switchMap(({ id, navigate }) => removeEquipmentApiService(id)
    .pipe(
      map(() => {
        navigate();
        return setLoadingAction(false);
      }),
    )),
);

const equipmentEpic = combineEpics(
  getEquipmentDetailsEpic,
  editEquipmentEpic,
  reserveEquipmentEpic,
  removeEquipmentEpic,
);

export default equipmentEpic;
