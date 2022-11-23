import { SummaryDto } from '../../services/summary/models';

export const GET_SUMMARY = 'GET_SUMMARY';
export const GET_SUMMARY_SUCCESS = 'GET_SUMMARY_SUCCESS';

export interface ISummaryState {
  summary: SummaryDto
}

export interface IGetSummary {
  type: typeof GET_SUMMARY;
}

export interface IGetSummarySuccess {
  type: typeof GET_SUMMARY_SUCCESS;
  payload: SummaryDto;
}

export type SummaryActionTypes = IGetSummary | IGetSummarySuccess;
