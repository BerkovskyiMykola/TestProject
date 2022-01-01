import React, { useEffect, useState } from "react";
import { useTranslation } from "react-i18next";

import { shallowEqual, useDispatch, useSelector } from "react-redux";
import { clearMessage } from "../../actions/message";
import { editProfile, getProfile } from "../../actions/profile";
import { validateField, validateRequired } from "../../validation/validation";
import { Field } from "../FormComponents";
import ModalWindow from "../ModalWindow/ModalWindow";
import { Row, Button, Col, Container, Jumbotron } from "reactstrap";

export default function Profile(props) {
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
            .then(() => { })
            .catch(() => { props.history.push("/404") });
    }, [dispatch, props.history])

    const editRecord = () => {
        dispatch(editProfile(model.lastname, model.firstname))
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
                        <Button color="success" onClick={() => { dispatch(clearMessage()); setModalEdit(true); setModel(profile); }}>
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
                <Field name="lastname" value={model}
                    setValue={(e) => { setModel({ ...model, "lastname": e.target.value }) }} validations={[validateRequired(t), validateField(t)]} />
                <Field name="firstname" value={model}
                    setValue={(e) => { setModel({ ...model, "firstname": e.target.value }) }} validations={[validateRequired(t), validateField(t)]} />
            </ModalWindow>
        </Container>
    );
}