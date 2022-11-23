import React, { useState, useRef, useEffect } from 'react';
import { useTranslation } from 'react-i18next';
import { useSelector } from 'react-redux';
import ReactToPrint from 'react-to-print';
import { Button } from 'semantic-ui-react';
import { RoomDetailsDto } from '../../services/room/models';
import { AppState } from '../../store';
import styles from './RoomPlanSection.module.scss';
import { RoomForProjectDto } from '../../services/project/models';

type Props = {
  roomItem: RoomDetailsDto | RoomForProjectDto;
  roomToolsVisible: boolean;
}

// eslint-disable-next-line
const ComponentToPrint = React.forwardRef((props, ref) => <div id={styles.pdf} ref={ref} dangerouslySetInnerHTML={{ __html: props.content?.innerHTML }} />);

const RoomPlanSection: React.FC<Props> = ({ roomItem }) => {
  const { loggedUserData } = useSelector((state: AppState) => state.global);
  const { t } = useTranslation();
  const componentRef = useRef();
  const roomStore = useSelector((state: AppState) => state.room);
  const [disabledDesks, setDisabledDesks] = useState([] as number[]);
  const [hotDesks, setHotDesks] = useState([] as number[]);
  const [occupiedDesks, setOccupiedDesks] = useState([] as Array[]);
  const [sharedDesks, setSharedDesks] = useState([] as Array[]);
  const [freeDesks, setFreeDesks] = useState([] as number[]);
  useEffect(() => {
    setDisabledDesks([]);
    setHotDesks([]);
    setOccupiedDesks([]);
    setSharedDesks([]);
    setFreeDesks([]);
    roomItem.desksInRoom.forEach((desk) => {
      if (!desk.isEnabled) {
        setDisabledDesks((prevState) => [...prevState, desk.number]);
      }
      if (desk.isHotDesk) {
        setHotDesks((prevState) => [...prevState, desk.number]);
      }
      if (desk.reservations.length > 0 && !desk.isHotDesk) {
        const bookedDays = [];
        desk.reservations.forEach((reservation) => {
          bookedDays.push(reservation.scheduledWeekdays.length);
        });
        if (bookedDays.reduce((a, b) => a + b) === 5) {
          const names = desk.reservations.map((r) => `${r.employee.name} ${r.employee.surname}`);
          setOccupiedDesks((prevState) => [...prevState, [desk.number, names]]);
        } else if (bookedDays.reduce((a, b) => a + b) < 5) {
          const names = desk.reservations.map((r) => `${r.employee.name} ${r.employee.surname}`);
          setSharedDesks((prevState) => [...prevState, [desk.number, names]]);
        }
      }
      if (desk.reservations.length === 0 && desk.isEnabled && !desk.isHotDesk) {
        setFreeDesks((prevState) => [...prevState, desk.number]);
      }
    });
  }, [roomItem]);
  const createNamesTable = (s, names) => {
    const heading = s.querySelector('h1');
    const table = document.createElement('table');
    const tbody = document.createElement('tbody');
    names.forEach((name) => {
      const row = document.createElement('tr');
      const rowData = document.createElement('td');
      rowData.innerText = name;
      row.appendChild(rowData);
      tbody.appendChild(row);
    });
    table.appendChild(tbody);
    heading.appendChild(table);
  };
  useEffect(() => {
    const tables = document.querySelectorAll('svg table');
    tables.forEach((table) => {
      table.parentElement?.removeChild(table);
    });
    const switches = document.querySelectorAll('switch');
    switches.forEach((s) => {
      const heading = s.querySelector('h1');
      if (heading && disabledDesks.includes(parseInt(heading.innerHTML, 10))) {
        s.parentElement.previousSibling.setAttribute('fill', '#bfbfbf');
      }
      if (heading && hotDesks.includes(parseInt(heading.innerHTML, 10))) {
        s.parentElement.previousSibling.setAttribute('fill', '#90c2e7');
      }
      occupiedDesks.forEach((od) => {
        if (heading && od[0] === parseInt(heading.innerHTML, 10)) {
          s.parentElement.previousSibling.setAttribute('fill', '#ffb2b2');
          if (od[1].length >= 2) {
            const shortName = od[1].map((name) => `${name[0]}. ${name.split(' ').reverse()[0]}`);
            createNamesTable(s, shortName);
          } else {
            createNamesTable(s, od[1]);
          }
        }
      });
      sharedDesks.forEach((sd) => {
        if (heading && sd[0] === parseInt(heading.innerHTML, 10)) {
          s.parentElement.previousSibling.setAttribute('fill', '#ffdb99');
          if (sd[1].length >= 2) {
            const shortName = sd[1].map((name) => `${name[0]}. ${name.split(' ').reverse()[0]}`);
            createNamesTable(s, shortName);
          } else {
            createNamesTable(s, sd[1]);
          }
        }
      });
      if (heading && freeDesks.includes(parseInt(heading.innerHTML, 10))) {
        s.parentElement.previousSibling.setAttribute('fill', '#a6e3b4');
      }
    });
  }, [disabledDesks, hotDesks, occupiedDesks, sharedDesks, freeDesks]);
  return (
    <>
      <div className={styles.header}>
        <div>
          <h3>{t('roomDetails.roomPlan')}</h3>
        </div>
        {loggedUserData.isUserAdmin() === true && (
          <ReactToPrint
            content={() => componentRef.current}
            trigger={() => <Button color="blue" basic circular icon="download" disabled={roomStore.isViewDisabled} />}
          />
        )}
      </div>
      <ComponentToPrint ref={componentRef} content={document.getElementById('content-to-print')} />
      <div id="content-to-print">
        <div>
          <h2>
            {roomItem.building.name} {roomItem.name}
          </h2>
        </div>
        {/* eslint-disable-next-line */}
        <div id={styles.svg} className="roomPlanSvg" dangerouslySetInnerHTML={{ __html: roomItem?.roomPlanInfo }} />
        <div id={styles.legend} className="legend">
          <div className={styles.line}>
            <div id={styles.free} className={styles.color} />
            {t('roomDetails.freeDesk')}
          </div>
          <div className={styles.line}>
            <div id={styles.shared} className={styles.color} />
            {t('roomDetails.shared')}
          </div>
          <div className={styles.line}>
            <div id={styles.occupied} className={styles.color} />
            {t('roomDetails.occupiedDesk')}
          </div>
          <div className={styles.line}>
            <div id={styles.hot} className={styles.color} />
            {t('roomDetails.hotDesk')}
          </div>
          <div className={styles.line}>
            <div id={styles.disabled} className={styles.color} />
            {t('roomDetails.disabledShort')}
          </div>
        </div>
      </div>
    </>
  );
};

export default RoomPlanSection;
