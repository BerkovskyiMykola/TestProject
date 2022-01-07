import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { clearMessage } from "../../actions/message";
import { editProfile, getProfile } from "../../actions/profile";
import ModalWindow from "../ModalWindow/ModalWindow";
import { FieldInput } from "../FormComponents";

import {
    Row,
    Col,
    Container,
    Button,
    Jumbotron
} from 'reactstrap';

export default function Profile() {
    const { t } = useTranslation();
    const dispatch = useDispatch();

    const [modalEdit, setModalEdit] = useState(false);
    const [model, setModel] = useState({ lastname: "", firstname: "" });

    const { profile, message } = useSelector(state => ({
        profile: state.profile.profile,
        message: state.message.message
    }), shallowEqual)

    useEffect(() => {
        dispatch(getProfile())
    }, [dispatch])

    const editRecord = () => {
        dispatch(editProfile(model.lastname, model.firstname, t))
            .then(() => {
                setModalEdit(false);
                dispatch(clearMessage());
            })
            .catch(() => { })
    }

    return (
        <Container>
            <Jumbotron className="bg-dark text-white">
                <Row>
                    <Col sm="10" className="text-left">
                        <h3>
                            <strong>{t("Profile")}: {profile.lastname} {profile.firstname}</strong>
                        </h3>
                    </Col>
                    <Col sm="2" className="text-right">
                        <Button color="success" onClick={() => { dispatch(clearMessage()); setModalEdit(true); setModel({ lastname: profile.lastname, firstname: profile.firstname }); }}>
                            <i className="bi-pencil-square" />
                        </Button>
                    </Col>
                </Row>
            </Jumbotron>
            <p>
                <strong>{t("email")}:</strong> {profile.email}
            </p>
            <p>
                <strong>{t("role")}:</strong> {profile.role}
            </p>
            <ModalWindow modal={modalEdit} deactiveModal={() => { setModalEdit(false); }} textHeader={t("Edit")}
                textButton={t("Edit")} method={editRecord} message={message}
            >
                <FieldInput name="firstname" model={model} setModel={setModel} minLength={2} maxLength={30} />
                <FieldInput name="lastname" model={model} setModel={setModel} minLength={2} maxLength={30} />
            </ModalWindow>
        </Container>
    );
}