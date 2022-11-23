import { render, screen } from '@testing-library/react';
import userEvent from '@testing-library/user-event';
import React from 'react';
import { Provider } from 'react-redux';
import { BrowserRouter } from 'react-router-dom';
import { createStore } from 'redux';
import Projects from '.';
import { rootReducer } from '../../store';

describe('Projects component', () => {
  test('renders filter if "Show Filters" button is clicked', async () => {
    const store = createStore(rootReducer);

    const mockDispatch = jest.fn();
    jest.mock('react-redux', () => ({
      useSelector: jest.fn(),
      useDispatch: () => mockDispatch,
    }));

    render(
      <Provider store={store}>
        <BrowserRouter>
          <Projects />
        </BrowserRouter>
      </Provider>,
    );

    const button = await screen.findByRole('button');

    userEvent.click(button);

    const element = screen.findByText('Lider');
    expect(element).toBeInTheDocument();
  });
});
