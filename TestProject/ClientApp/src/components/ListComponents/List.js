import React from 'react'
import {  useTranslation } from 'react-i18next';
import { Table, Container, Row, Col } from "reactstrap";
import ListItem from './ListItem';


const List = ({ recorts, columns, k, deleteRecord, editRecord, openPage, action = true }) => {

    const { t } = useTranslation();

    if (recorts.length === 0) {
        return (
            <Container style={{ backgroundColor: "#F2F2F2" }}>
                <Row className="text-center">
                    <Col className="col-12 my-5"><h2>{t("ListEmpty")}</h2></Col>
                </Row>
            </Container>
        );
    }

    return (
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
                {recorts.map((item, index) => (<ListItem key={item[k]} item={item} index={index} columns={columns} deleteRecord={deleteRecord} editRecord={editRecord} openPage={openPage} action={action} />))}
            </tbody>
        </Table>
    );
};

export default List;