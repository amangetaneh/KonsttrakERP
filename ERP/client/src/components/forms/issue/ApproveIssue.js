import React from "react";
import PropTypes from "prop-types";
import IssueApproveMaterial from "./fragments/IssueApproveMaterial";
import { Button, Spinner } from "react-bootstrap";
import { approveIssue, declineIssue } from "../../../api/issue";
import { useMutation, useQueryClient } from "react-query";
import ConnectionError from "../../fragments/ConnectionError";

function ApproveIssue({ addedItems, setAddedItems, issue }) {
    const queryClient = useQueryClient();

    const {
        isLoading: isApproveLoading,
        isError: isApproveError,
        mutate: submitApproveIssue,
    } = useMutation(approveIssue, {
        onSuccess: (res) => queryClient.invalidateQueries(["issue", issue.issueId.toString()]),
    });

    const {
        isLoading: isDeclineLoading,
        isError: isDeclineError,
        mutate: submitDeclineIssue,
    } = useMutation(declineIssue, {
        onSuccess: (res) => queryClient.invalidateQueries(["issue", issue.issueId.toString()]),
    });

    function submit(type, e) {
        e.preventDefault();
        if (isApproveLoading || isDeclineLoading) return;

        const data = {
            issueId: issue.issueId,
            approvedById: 1, ////////////////////////
            issueItems: [],
        };

        for (let item of addedItems) {
            let tempItem = {
                itemId: Number(item.itemId),
                qtyApproved: Number(item.qtyApproved),
                approveRemark: item.approveRemark,
            };

            data.issueItems.push(tempItem);
        }

        if (type === "decline") submitDeclineIssue(data);
        else submitApproveIssue(data);
    }

    if (isApproveError || isDeclineError) return <ConnectionError />;

    return (
        <>
            {addedItems.map((item, index) => (
                <IssueApproveMaterial
                    key={index}
                    addedItems={addedItems}
                    setAddedItems={setAddedItems}
                    index={index}
                />
            ))}

            <div className="row">
                <div className="col-8 d-grid">
                    <Button className="btn-teal-dark" onClick={submit.bind(this, "approve")}>
                        {isApproveLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Approve
                    </Button>
                </div>
                <div className="col-4 d-grid">
                    <Button variant="danger" onClick={submit.bind(this, "decline")}>
                        {isDeclineLoading && (
                            <Spinner className="me-2" animation="grow" size="sm" />
                        )}
                        Decline
                    </Button>
                </div>
            </div>
        </>
    );
}

ApproveIssue.propTypes = {
    addedItems: PropTypes.array.isRequired,
    setAddedItems: PropTypes.func.isRequired,
    issue: PropTypes.object.isRequired,
};

export default ApproveIssue;
