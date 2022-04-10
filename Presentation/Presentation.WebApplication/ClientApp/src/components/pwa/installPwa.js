import React from 'react';
import { Button, Modal, Card, CardText, CardBody, CardTitle } from 'reactstrap';
import useIosInstallPrompt from '../../hooks/pwa/iosInstallPrompt';
import useWebInstallPrompt from '../../hooks/pwa/webInstallPrompt';

export const InstallPwa = () => {
    const [iosInstallPrompt, handleIOSInstallDeclined] = useIosInstallPrompt();
    const [webInstallPrompt, handleWebInstallDeclined, handleWebInstallAccepted] = useWebInstallPrompt();

    if (!iosInstallPrompt && !webInstallPrompt) {
        return null;
    }
    return (
        <Modal isOpen centered>
            <Card>
                <img
                    className="mx-auto"
                    style={{
                        borderTopRightRadius: '50%',
                        borderTopLeftRadius: '50%',
                        backgroundColor: '#fff',
                        marginTop: '-50px'
                    }}
                    width="100px"
                    src="imgs/icons/android/android-launchericon-512-512.png"
                    alt="Icon"
                />
                <CardBody>
                    <CardTitle className="text-center">
                        <h3>Instalar App</h3>
                    </CardTitle>
                    {iosInstallPrompt && (
                        <>
                            <CardText className="text-center">
                                Tap
                                <img
                                    src="imgs/icons/Navigation_Action.png"
                                    style={{ margin: 'auto 8px 8px' }}
                                    className=""
                                    alt="Add to homescreen"
                                    width="20"
                                />
                                then &quot;Add to Home Screen&quot;
                            </CardText>
                            <div className="d-flex justify-content-center">
                                <Button onClick={handleIOSInstallDeclined}>Cerrar</Button>
                            </div>
                        </>
                    )}
                    {webInstallPrompt && (
                        <div className="d-flex justify-content-around">
                            <Button color="primary" onClick={handleWebInstallAccepted}>
                                Instalar
                            </Button>
                            <Button onClick={handleWebInstallDeclined}>Cerrar</Button>
                        </div>
                    )}
                </CardBody>
            </Card>
        </Modal>
    );
};