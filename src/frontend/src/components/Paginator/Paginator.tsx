import React, { useEffect, useState } from 'react';
import { useTranslation } from 'react-i18next';
import { Dropdown, DropdownItemProps, DropdownProps, Grid, Icon,
  Pagination, PaginationProps, Responsive } from 'semantic-ui-react';
import styles from './Paginator.module.scss';
import { LAPTOP_MEDIA_WIDTH } from '../../globalConstants';

type Props = {
  initialPageNumber: number;
  initialPageSize: number;
  recordCount: number;
  onPageChange: (activePage: number, pageSize: number) => void;
}

const Paginator: React.FC<Props> = ({ initialPageNumber, initialPageSize, recordCount, onPageChange }) => {
  const pageSizeOptions: DropdownItemProps[] = [
    { value: 10, text: 10 },
    { value: 20, text: 20 },
    { value: 50, text: 50 },
    { value: 100, text: 100 },
  ];

  if (!pageSizeOptions.some((x) => x.value === initialPageSize)) {
    pageSizeOptions.push({ value: initialPageSize, text: initialPageSize });
  }

  const [activePage, setActivePage] = useState(1);
  const [pageSize, setPageSize] = useState(initialPageSize);
  const { t } = useTranslation();
  const pageCount = Math.ceil(recordCount / pageSize);

  const firstItemProps = { content: <Icon name="angle double left" />, icon: true };
  const lastItemProps = { content: <Icon name="angle double right" />, icon: true };
  const prevItemProps = { content: <Icon name="angle left" />, icon: true };
  const nextItemProps = { content: <Icon name="angle right" />, icon: true };

  useEffect(() => {
    setActivePage(initialPageNumber);
  }, [initialPageNumber]);

  const handlePaginationChange =
    (_: any, props: PaginationProps) => {
      const page = props.activePage as number;
      setActivePage(page);
      onPageChange(page, pageSize);
    };

  const handlePageSizeChange =
    (_: any, props: DropdownProps) => {
      const size = props.value as number;
      setPageSize(size);
      setActivePage(1);
      onPageChange(1, size);
    };

  const pageSizeOptionDropdown = (
    <div>
      {`${t('common.list.displayCountPrefix')} `}
      <Dropdown
        options={pageSizeOptions}
        defaultValue={initialPageSize}
        onChange={handlePageSizeChange}
        inline
      />
      {` ${t('common.list.displayCountSuffix')}`}
    </div>
  );

  const pagination = (
    <Pagination
      activePage={activePage}
      boundaryRange={1}
      siblingRange={1}
      totalPages={pageCount}
      onPageChange={handlePaginationChange}
      ellipsisItem={{ disabled: true, content: <Icon id={styles.elipsisIcon} name="ellipsis horizontal" />, icon: true }}
      firstItem={
      activePage !== 1 ?
        firstItemProps :
        { ...firstItemProps, disabled: true }
      }
      prevItem={
      activePage !== 1 ?
        prevItemProps :
        { ...prevItemProps, disabled: true }
      }
      nextItem={
      activePage !== pageCount ?
        nextItemProps :
        { ...nextItemProps, disabled: true }
      }
      lastItem={
      activePage !== pageCount ?
        lastItemProps :
        { ...lastItemProps, disabled: true }
      }
      secondary
    />
  );
  return (
    <>
      <Responsive as={Grid} minWidth={LAPTOP_MEDIA_WIDTH} verticalAlign="middle">
        <Grid.Row>
          <Grid.Column className={styles.fitContent}>
            {pageSizeOptionDropdown}
          </Grid.Column>
          <Grid.Column textAlign="center" className={styles.stretch}>
            {pagination}
          </Grid.Column>
          <Grid.Column textAlign="right" className={styles.fitContent}>
            {`${t('common.list.recordCount')} ${recordCount}`}
          </Grid.Column>
        </Grid.Row>
      </Responsive>
      <Responsive as={Grid} maxWidth={LAPTOP_MEDIA_WIDTH - 1} verticalAlign="middle">
        <Grid.Row className={styles.row}>
          <Grid.Column className={styles.fitContent}>
            {pageSizeOptionDropdown}
          </Grid.Column>
          <Grid.Column textAlign="right" className={styles.fitContent}>
            {`${t('common.list.recordCount')} ${recordCount}`}
          </Grid.Column>
        </Grid.Row>
        <Grid.Row colSpan={2}>
          <Grid.Column textAlign="center" className={styles.stretch}>
            {pagination}
          </Grid.Column>
        </Grid.Row>
      </Responsive>
    </>
  );
};

export default Paginator;
