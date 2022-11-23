import React from 'react';
import { useLocation, useHistory, NavLink, Link } from 'react-router-dom';
import { Menu, Icon, Responsive, Dropdown, IconGroup, Image, Item } from 'semantic-ui-react';
import { useTranslation } from 'react-i18next';
import { useSelector, useDispatch } from 'react-redux';
import { Routes } from '../../../Routes';
import { AppState } from '../../../store';
import UsersSearcher from '../../../components/UsersSearcher/UsersSearcher';
import styles from './Navbar.module.scss';
import { MOBILE_MEDIA_WIDTH, MEDIUM_MEDIA_WIDTH } from '../../../globalConstants';
import { UserRole } from '../../../services/user/enums';
import i18n from '../../../i18n';
import { toggleDarkMode } from '../../../store/darkMode/actions';

const Navbar: React.FC = () => {
  const { pathname } = useLocation();
  const { t } = useTranslation();
  const history = useHistory();
  const dispatch = useDispatch();
  const { loggedUserData } = useSelector((state: AppState) => state.global);

  const languageOptions = [
    {
      key: 'pl',
      text: 'Polski',
      value: 'pl-PL',
      image: {
        circular: true,
        size: 'tiny',
        src: 'https://hatscripts.github.io/circle-flags/flags/pl.svg',
      },
    },
    {
      key: 'en',
      text: 'English',
      value: 'en-US',
      image: {
        circular: true,
        size: 'tiny',
        src: 'https://hatscripts.github.io/circle-flags/flags/gb.svg',
      },
    },
  ];

  const handleOnChangeLanguage = (e: any, data: any) => {
    i18n.changeLanguage(data.value.toString());
  };

  const mode = useSelector((state: AppState) => state.darkMode);
  const { darkMode } = mode;

  const switchDarkMode = () => dispatch(toggleDarkMode(!darkMode));

  const routesHamburger = (
    <Dropdown item icon="bars" id={styles.dropdownRoutes}>
      <Dropdown.Menu id={styles.dropdownMenu}>
        {!loggedUserData.isStandardUser() && (
          <Dropdown.Item id={pathname === Routes.Floors ? styles.active : ''}>
            <NavLink activeClassName={styles.active} to={Routes.Floors} exact>{t('floors.header')} </NavLink>
          </Dropdown.Item>
        )}
        <Dropdown.Item id={pathname === Routes.Rooms ? styles.active : ''}>
          <NavLink activeClassName={styles.active} to={Routes.Rooms}>{t('rooms.header')}</NavLink>
        </Dropdown.Item>
        <Dropdown.Item id={pathname === Routes.HotDesks ? styles.active : ''}>
          <NavLink activeClassName={styles.active} to={Routes.HotDesks}>{t('hotDesks.header')}</NavLink>
        </Dropdown.Item>
        {!loggedUserData.isStandardUser() && (
          <Dropdown.Item id={pathname === Routes.Projects ? styles.active : ''}>
            <NavLink activeClassName={styles.active} to={Routes.Projects}>{t('projects.header')}</NavLink>
          </Dropdown.Item>
        )}
        {loggedUserData.hasRole(UserRole.Admin) && (
          <Dropdown.Item id={pathname === Routes.ExternalCompanies ? styles.active : ''}>
            <NavLink activeClassName={styles.active} to={Routes.ExternalCompanies}>{t('externalCompanies.header')}</NavLink>
          </Dropdown.Item>
        )}
        {loggedUserData.hasRole(UserRole.Admin) && (
          <Menu.Item active={pathname === Routes.Equipments}>
            <NavLink activeClassName={styles.active} to={Routes.Equipments}>{t('equipments.header')}</NavLink>
          </Menu.Item>
        )}
        {!loggedUserData.isStandardUser() && (
          <Dropdown.Item id={pathname === Routes.Summary ? styles.active : ''}>
            <NavLink activeClassName={styles.active} to={Routes.Summary}>{t('summary.header')}</NavLink>
          </Dropdown.Item>
        )}
        {!loggedUserData.isStandardUser() && (
          <Dropdown.Item>
            <Item
              className={styles.link}
              activeClassName={styles.active}
              href="https://futureprocessinguk.sharepoint.com/sites/TAM"
              target="_blank"
            >
              {t('common.help')}
            </Item>
          </Dropdown.Item>
        )}
        <Dropdown.Item id={styles.toggleBtn}>
          <Icon
            color="yellow"
            name={darkMode ? 'moon' : 'sun'}
          />
          <Icon
            color="blue"
            className={styles.toggleDark}
            name={darkMode ? 'toggle on' : 'toggle off'}
            size="massive"
            onClick={switchDarkMode}
          />
        </Dropdown.Item>
      </Dropdown.Menu>
    </Dropdown>
  );

  const routesInline = (
    <>
      {!loggedUserData.isStandardUser() && (
        <Menu.Item active={pathname === Routes.Floors}>
          <NavLink activeClassName={styles.active} to={Routes.Floors} exact>{t('floors.header')}</NavLink>
        </Menu.Item>
      )}
      <Menu.Item active={pathname === Routes.Rooms}>
        <NavLink activeClassName={styles.active} to={Routes.Rooms}>{t('rooms.header')}</NavLink>
      </Menu.Item>
      <Menu.Item active={pathname === Routes.HotDesks}>
        <NavLink activeClassName={styles.active} to={Routes.HotDesks}>{t('hotDesks.header')}</NavLink>
      </Menu.Item>
      {!loggedUserData.isStandardUser() && (
        <Menu.Item active={pathname === Routes.Projects}>
          <NavLink activeClassName={styles.active} to={Routes.Projects}>{t('projects.header')}</NavLink>
        </Menu.Item>
      )}
      {loggedUserData.hasRole(UserRole.Admin) && (
        <Menu.Item active={pathname === Routes.ExternalCompanies}>
          <NavLink activeClassName={styles.active} to={Routes.ExternalCompanies}>{t('externalCompanies.header')}</NavLink>
        </Menu.Item>
      )}
      {loggedUserData.hasRole(UserRole.Admin) && (
        <Menu.Item active={pathname === Routes.Equipments}>
          <NavLink activeClassName={styles.active} to={Routes.Equipments}>{t('equipments.header')}</NavLink>
        </Menu.Item>
      )}
      {!loggedUserData.isStandardUser() && (
        <Menu.Item active={pathname === Routes.Summary}>
          <NavLink activeClassName={styles.active} to={Routes.Summary}>{t('summary.header')}</NavLink>
        </Menu.Item>
      )}
      {!loggedUserData.isStandardUser() && (
        <Menu.Item>
          <Item
            className={styles.link}
            activeClassName={styles.active}
            href="https://futureprocessinguk.sharepoint.com/sites/TAM"
            target="_blank"
          >
            {t('common.help')}
          </Item>
        </Menu.Item>
      )}
    </>
  );

  const userAsAvatar = (
    <Dropdown
      item
      id={styles.dropdown}
      icon={(
        <IconGroup>
          <Icon name="user" />
          <Icon className={styles.caretDown} name="caret down" />
        </IconGroup>
      )}
    >
      <Dropdown.Menu direction="left">
        <Dropdown.Item text={loggedUserData.email} />
        {loggedUserData.isUserAdmin() === true && (
          <Dropdown.Item as={Link} to={Routes.Admin}>
            <Icon name="cogs" color="green" />{t('admin.adminPanel')}
          </Dropdown.Item>
        )}
      </Dropdown.Menu>
    </Dropdown>
  );

  const langSelector = (
    <Dropdown
      item
      id={styles.langSelector}
      icon={(
        <IconGroup>
          <Icon name="globe" />
          <Icon className={styles.caretDown} name="caret down" />
        </IconGroup>
      )}
    >
      <Dropdown.Menu className={styles.langSelectorMenu}>
        {languageOptions.map((l) => (
          <Dropdown.Item
            key={l.key}
            text={l.text}
            value={l.value}
            image={l.image}
            onClick={handleOnChangeLanguage}
          />
        ))}
      </Dropdown.Menu>
    </Dropdown>
  );

  return (
    <>
      {/* Menu if screen width bigger than mobile */}
      <Responsive minWidth={MOBILE_MEDIA_WIDTH}>
        <Menu inverted borderless fixed="top" id={styles.navbar} className={darkMode ? `${styles.darkNavbar}` : undefined}>
          <Menu.Item className={styles.iconMenu}>
            <NavLink to={Routes.UserDashboard}>
              <Image className={`${styles.inline} ${styles.icon}`} src="/ms-icon-70x70.png" />
            </NavLink>
          </Menu.Item>
          <Responsive as={Menu.Menu} minWidth={MEDIUM_MEDIA_WIDTH}>
            {routesInline}
          </Responsive>
          <Responsive as={Menu.Menu} maxWidth={MEDIUM_MEDIA_WIDTH - 1}>
            {routesHamburger}
          </Responsive>
          <Menu.Menu position="right">
            <div id={styles.inline}>
              <UsersSearcher />
              {userAsAvatar}
              {langSelector}
              <Responsive as={Item} minWidth={MEDIUM_MEDIA_WIDTH}>
                <Icon
                  color="yellow"
                  name={darkMode ? 'moon' : 'sun'}
                />
                <Icon
                  color="blue"
                  className={styles.toggleDark}
                  name={darkMode ? 'toggle on' : 'toggle off'}
                  size="big"
                  onClick={switchDarkMode}
                  title={darkMode ? t('colorScheme.switchMode') + t('colorScheme.lightMode') : t('colorScheme.switchMode') + t('colorScheme.darkMode')}
                />
              </Responsive>
            </div>
          </Menu.Menu>
        </Menu>
      </Responsive>

      {/* Menu if screen width smaller than mobile */}
      <Responsive maxWidth={MOBILE_MEDIA_WIDTH - 1}>
        <Menu inverted borderless fixed="top" stackable id={styles.navbar}>
          <div id={styles.inlineSpaceBetween}>
            <div id={styles.inline}>
              <Image className={`${styles.inline} ${styles.icon}`} src="/ms-icon-70x70.png" onClick={() => history.push(Routes.UserDashboard)} />
              <Menu.Menu>
                {routesHamburger}
              </Menu.Menu>
            </div>
            <UsersSearcher />
            {userAsAvatar}
          </div>
        </Menu>
      </Responsive>
    </>
  );
};

export default Navbar;
