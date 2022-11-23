import React, { useEffect } from 'react';
import { Switch, Route } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { MsalAuthProvider } from 'react-aad-msal';
import Layout from './layouts/Layout';
import Floors from './pages/Floors';
import ProjectDetails from './pages/ProjectDetails';
import Projects from './pages/Projects';
import Room from './pages/RoomDetails';
import Rooms from './pages/Rooms';
import { Routes } from './Routes';
import { getLoggedUserDataAction, setLoadingAction } from './store/global/actions';
import PersonDetails from './pages/UserDetails';
import Admin from './pages/Admin';
import ExternalCompanies from './pages/ExternalCompanies';
import ExternalCompaniesDetails from './pages/ExternalCompaniesDetails';
import Summary from './pages/Summary';
import HotDesks from './pages/HotDesks';
import HotDeskDetails from './pages/HotDeskDetails';
import AuthGuard from './components/AuthGuard/AuthGuard';
import { UserRole } from './services/user/enums';
import UserDashboard from './pages/UserDashboard/index';
import Equipments from './pages/Equipments';
import EquipmentsDetails from './pages/EquipmentsDetails';
import PageNotFound from './pages/PageNotFound';

const App: React.FC<{ provider: MsalAuthProvider}> = ({ provider }) => {
  const dispatch = useDispatch();
  const loggedUsername = provider.getAccountInfo()?.account.userName as string;
  useEffect(() => {
    dispatch(setLoadingAction(true));
    dispatch(getLoggedUserDataAction());
  }, [loggedUsername]);

  return (
    <Layout>
      <Switch>
        <Route path={Routes.Rooms} exact component={Rooms} />
        <Route path={Routes.RoomDetails} exact render={(props) => <Room id={props.match.params.id as string} />} />
        <Route
          path={Routes.ProjectDetails}
          exact
          render={(props) => (
            <AuthGuard userRole={UserRole.TeamLeader}>
              <ProjectDetails id={props.match.params.id as string} {...props} />
            </AuthGuard>
          )}
        />
        <Route path={Routes.Floors} exact component={() => <AuthGuard userRole={UserRole.TeamLeader}> <Floors /> </AuthGuard>} />
        <Route path={Routes.Projects} exact render={() => <AuthGuard userRole={UserRole.TeamLeader}> <Projects /> </AuthGuard>} />
        <Route
          path={Routes.UserDetails}
          exact
          render={(props) => <PersonDetails userId={props.match.params.id as string} />}
        />
        <Route path={Routes.Summary} exact render={() => <AuthGuard userRole={UserRole.TeamLeader}> <Summary /> </AuthGuard>} />
        <Route path={Routes.Admin} exact component={() => <AuthGuard userRole={UserRole.Admin}> <Admin /> </AuthGuard>} />
        <Route path={Routes.HotDesks} exact component={() => <HotDesks />} />
        <Route path={Routes.HotDeskDetails} exact component={(props) => <HotDeskDetails id={props.match.params.id as string} />} />
        <Route
          path={Routes.ExternalCompaniesDetails}
          exact
          render={(props) => (
            <AuthGuard userRole={UserRole.Admin}>
              <ExternalCompaniesDetails companyId={props.match.params.id as string} {...props} />
            </AuthGuard>
          )}
        />
        <Route path={Routes.ExternalCompanies} exact component={() => <AuthGuard userRole={UserRole.Admin}><ExternalCompanies /> </AuthGuard>} />
        <Route path={Routes.Equipments} exact component={() => <AuthGuard userRole={UserRole.Admin}> <Equipments /> </AuthGuard>} />
        <Route
          path={Routes.EquipmentDetails}
          exact
          render={(props) => (
            <AuthGuard userRole={UserRole.Admin}>
              <EquipmentsDetails equipmentId={props.match.params.id as string} {...props} />
            </AuthGuard>
          )}
        />
        <Route path={Routes.UserDashboard} exact component={() => <UserDashboard />} />
        <Route component={() => <PageNotFound />} />
      </Switch>
    </Layout>
  );
};

export default App;
