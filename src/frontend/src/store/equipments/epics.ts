import { combineEpics, Epic } from 'redux-observable';
import { of } from 'rxjs';
import { catchError, switchMap } from 'rxjs/operators';
import i18n from 'i18next';
import { ErrorCodes } from '../../services/common/models';
import addEquipmentApiService from '../../services/equipments/addEquipmentApiService';
import getAllEquipmentsApiService from '../../services/equipments/getAllEquipmentsApiService';
import { setLoadingAction, showErrorCodeInNotifyAction, showNotifyAction } from '../global/actions';
import { addEquipmentSuccess, closeAddEquipmentModalAction, fetchEquipmentsSuccess } from './actions';
import { ADD_EQUIPMENT, FETCH_EQUIPMENTS } from './types';

const equipmentsEpic: Epic = (action$) =>
  action$.ofType(FETCH_EQUIPMENTS)
    .pipe(
      switchMap(() => getAllEquipmentsApiService().pipe(
        switchMap((response) => of(
          fetchEquipmentsSuccess(response),
          setLoadingAction(false),
        )),
        handleApiErrorAndStopLoader(),
      )),
    );

const addEquipmentEpic: Epic = (action$) =>
  action$.ofType(ADD_EQUIPMENT)
    .pipe(
      switchMap(({ addEquipmentDto }) => addEquipmentApiService(addEquipmentDto)
        .pipe(
          switchMap((returnData) => of(
            addEquipmentSuccess(addEquipmentDto, returnData),
            closeAddEquipmentModalAction(),
            setLoadingAction(false),
          )),
          handleApiErrorAndStopLoader(),
        )),
    );

const handleApiErrorAndStopLoader = () =>
  catchError((error: any) => {
    if (error.name || error.name === 'AjaxError') {
      const errorMessage = error.response.translationKey ? i18n.t(error.response.translationKey) : error.response.message;
      return of(showNotifyAction(errorMessage, true, ' '), setLoadingAction(false));
    }
    return of(showErrorCodeInNotifyAction(ErrorCodes.UnknownError), setLoadingAction(false));
  });

const equipmentsEpics = combineEpics(equipmentsEpic, addEquipmentEpic);
export default equipmentsEpics;
