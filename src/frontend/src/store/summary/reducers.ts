import { SummaryDto } from '../../services/summary/models';
import { GET_SUMMARY_SUCCESS, ISummaryState, SummaryActionTypes } from './types';

const initialState: ISummaryState = {
  summary: {} as SummaryDto,
};

export function summaryReducer(state = initialState, action: SummaryActionTypes): ISummaryState {
  switch (action.type) {
    case GET_SUMMARY_SUCCESS:
      return { ...state, summary: action.payload };
    default:
      return state;
  }
}
