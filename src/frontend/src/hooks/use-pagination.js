import React, { useEffect, useState } from 'react';
import { Button, Container } from 'semantic-ui-react';

const usePagination = (dataLength, filteredData) => {
  const [currentPageNo, setCurrentPageNo] = useState(1);
  const pageSize = 10;
  // console.log('currentPageNo', currentPageNo);
  const [paginatedFilteredData, setPaginatedFilteredData] = useState(filteredData);
  console.log('pageSize', pageSize);
  const lastPageNo = Math.ceil(dataLength / pageSize);
  console.log('dataLength tu', dataLength);
  const [startFromIndex, setStartFromIndex] = useState();
  // const [dataLength, setDataLength] = useState(30);

  useEffect(() => {
    setStartFromIndex((currentPageNo - 1) * pageSize);
    // console.log('setStartFromIndex', setStartFromIndex);
  }, [currentPageNo, pageSize]);
  useEffect(() => {
    setPaginatedFilteredData(
      // setDataLength(
      //   rooms?.roomsResponse?.rooms.length,
      // );
      filteredData.slice(startFromIndex, pageSize * currentPageNo),
    );
  }, [startFromIndex, currentPageNo, pageSize, filteredData]);

  const goToFirstPageHandler = () => {
    setCurrentPageNo(1);
  };
  const goToPrevPageHandler = () => {
    setCurrentPageNo((state) => state - 1);
  };
  const goToNextPageHandler = () => {
    setCurrentPageNo((state) => state + 1);
  };
  const goToLastPageHandler = () => {
    setCurrentPageNo(lastPageNo);
  };

  return (
    {
      currentPageNo,
      pageSize,
      startFromIndex,
      paginatedFilteredData,
      pagination: (
        <Container textAlign="center">
          <Button disabled={currentPageNo === 1} onClick={goToFirstPageHandler}>
            First Page
          </Button>
          <Button disabled={currentPageNo === 1} onClick={goToPrevPageHandler}>
            {currentPageNo === 1 ? '...' : currentPageNo - 1}
          </Button>
          <Button>
            <h4>{currentPageNo}</h4>
          </Button>
          <Button disabled={currentPageNo === lastPageNo} onClick={goToNextPageHandler}>
            {currentPageNo === lastPageNo ? '...' : currentPageNo + 1}
          </Button>
          <Button disabled={currentPageNo === lastPageNo} onClick={goToLastPageHandler}>
            Last Page
          </Button>
          <span>Total pages: {lastPageNo}</span>
        </Container>
      ),
    }
  );
};

export default usePagination;
