import React, { useEffect, useState } from 'react';
import { Button, Container } from 'semantic-ui-react';

const usePagination = (filteredData, pageSize = 10) => {
  const [currentPageNo, setCurrentPageNo] = useState(1);
  const [paginatedFilteredData, setPaginatedFilteredData] = useState(filteredData);
  const [startFromIndex, setStartFromIndex] = useState();
  const [dataLength, setDataLength] = useState(30);
  const lastPageNo = Math.ceil(dataLength / pageSize);

  useEffect(() => {
    setStartFromIndex((currentPageNo - 1) * pageSize);
  }, [currentPageNo, pageSize]);
  useEffect(() => {
    setDataLength(filteredData.length);
    setPaginatedFilteredData(
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
      paginatedFilteredData,
      paginationNavigation: (
        <Container textAlign="center">
          {currentPageNo !== 1 && currentPageNo !== 2 && (
            <Button disabled={currentPageNo === 1 || currentPageNo === 2} onClick={goToFirstPageHandler}>
              Go to the First Page (1)
            </Button>
          )}
          {currentPageNo !== 1 && (
            <Button disabled={currentPageNo === 1} onClick={goToPrevPageHandler}>
              {currentPageNo - 1}
            </Button>
          )}
          <Button>
            <h4>{currentPageNo}</h4>
          </Button>
          {currentPageNo !== lastPageNo && (
            <Button disabled={currentPageNo === lastPageNo} onClick={goToNextPageHandler}>
              {currentPageNo + 1}
            </Button>
          )}
          {currentPageNo !== lastPageNo && currentPageNo !== lastPageNo - 1 && (
            <Button disabled={currentPageNo === lastPageNo || currentPageNo === lastPageNo - 1} onClick={goToLastPageHandler}>
              Go to the Last Page ({lastPageNo})
            </Button>
          )}
        </Container>
      ),
    }
  );
};

export default usePagination;
