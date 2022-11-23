import React, { Component } from 'react';
import i18next from 'i18next';
import { Segment } from 'semantic-ui-react';

interface Props {}

interface State {
  hasError: boolean;
}

class ErrorBoundary extends Component<Props, State> {
  constructor(props: any) {
    super(props);
    this.state = { hasError: false };
  }
  static getDerivedStateFromError() {
    // Update state so the next render will show the fallback UI.
    return { hasError: true };
  }
  componentDidCatch() {
    // You can also log the error to an error reporting service
    // logErrorToMyService(error, errorInfo);
  }
  render() {
    const { hasError } = this.state;
    const { children } = this.props;

    if (hasError) {
      // You can render any custom fallback UI
      return (
        <>
          <Segment color="red" textAlign="center">
            {`${i18next.t('common.errorOccurred')}. ${i18next.t('common.contactAdmin')}.`}
          </Segment>
        </>
      );
    }

    return children;
  }
}

export default ErrorBoundary;
