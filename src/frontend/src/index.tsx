import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import './index.scss';
import './i18n';
import { AzureAD } from 'react-aad-msal';
import { BrowserRouter as Router } from 'react-router-dom';
import configureStore from './store';
import { authProvider } from './auth/authProvider';
import App from './App';

const store = configureStore();
const Root = () => (
  <Provider store={store}>
    <AzureAD provider={authProvider} forceLogin>
      <Router>
        <App provider={authProvider} />
      </Router>
    </AzureAD>
  </Provider>
);

ReactDOM.render(<Root />, document.getElementById('root'));
