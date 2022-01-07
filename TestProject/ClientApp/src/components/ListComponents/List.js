import React from 'react'
import { useTranslation } from 'react-i18next';
import { Table, Container, Row, Col, Button } from "reactstrap";
import ListItem from './ListItem';


const List = ({ name, records, columns, k, createRecord, refreshRecords, action }) => {

    const { t } = useTranslation();

    if (records.length === 0) {
        return (
            <>
                <Container>
                    <Row>
                        <Col className="text-left"><h3>{t(name)}</h3></Col>
                        <Col className="text-right">
                            {createRecord && <Button onClick={() => createRecord()} color="success">{t("Create")}</Button>}
                            {refreshRecords && 
                                <Button style={{ marginLeft: "3px" }} onClick={() => refreshRecords()}>
                                    <i className="bi-arrow-clockwise" />
                                </Button>
                            }
                        </Col>
                    </Row>
                </Container>
                <Container style={{ backgroundColor: "#F2F2F2" }}>
                    <Row className="text-center">
                        <Col className="col-12 my-5"><h2>{t("ListEmpty")}</h2></Col>
                    </Row>
                </Container>
            </>
        );
    }

    return (
        <>
            <Container>
                <Row>
                    <Col className="text-left"><h3>{t(name)}</h3></Col>
                    <Col className="text-right">
                        {createRecord && <Button onClick={() => createRecord()} color="success">{t("Create")}</Button>}
                        {refreshRecords &&
                            <Button style={{ marginLeft: "3px" }} onClick={() => refreshRecords()}>
                                <i className="bi-arrow-clockwise" />
                            </Button>
                        }
                    </Col>
                </Row>
            </Container>
            <Table style={{ marginTop: '5px', borderCollapse: 'collapse', borderRadius: '0.3em', overflow: 'hidden' }} dark bordered>
                <thead>
                    <tr>
                        <th>#</th>
                        {columns.map((x, index) =>
                            <th key={index}>{t(x)}</th>
                        )}
                        {action && <th>{t("Actions")}</th>}
                    </tr>
                </thead>
                <tbody>
                    {records.map((item, index) => (<ListItem key={item[k]} item={item} index={index} columns={columns} action={action && action(item)} />))}
                </tbody>
            </Table>
        </>
    );
};

export default List;