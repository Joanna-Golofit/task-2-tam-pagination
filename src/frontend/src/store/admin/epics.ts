import { Epic, combineEpics } from 'redux-observable';
import { filter, switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import i18n from 'i18next';
import { IMPORT_BUBBLES } from './types';
import { importBubblesSuccessAction } from './actions';
import executeImportFromBubblesApiService from '../../services/import/executeImportFromBubblesApiService';
import { showNotifyAction } from '../global/actions';
import { ImportReportDto } from '../../services/import/models';

const importBubblesEpic: Epic = (action$) => action$.pipe(
  filter((action) => action.type === IMPORT_BUBBLES),
  switchMap(() => executeImportFromBubblesApiService()
    .pipe(
      switchMap((dto: ImportReportDto) => of(importBubblesSuccessAction(),
        showNotifyAction(
          `${i18n.t('admin.removedEmptyProjectsCount')}: ${dto.removedEmptyProjectsCount},
          ${i18n.t('admin.removedEmployeesWithoutProjectsCount')}: ${dto.removedEmployeesWithoutProjectsCount},
          ${i18n.t('admin.usersBeforeImportCount')}: ${dto.usersBeforeImportCount},
          ${i18n.t('admin.usersAfterImportCount')}: ${dto.usersAfterImportCount},
          ${i18n.t('admin.projectsBeforeImportCount')}: ${dto.projectsBeforeImportCount},
          ${i18n.t('admin.projectsAfterImportCount')}: ${dto.projectsAfterImportCount},
          ${i18n.t('admin.assignmentsBeforeImportCount')}: ${dto.assignmentsBeforeImportCount},
          ${i18n.t('admin.assignmentsAfterImportCount')}: ${dto.assignmentsAfterImportCount},
          ${i18n.t('admin.removedEmployeesFromFutureAssigmentsCount')}: ${dto.removedEmployeesFromFutureAssigmentsCount},`,
          false,
          `${i18n.t('admin.importFromBubblesFinished')}`,
        ))),
    )),
);

const adminEpics = combineEpics(importBubblesEpic);

export default adminEpics;
