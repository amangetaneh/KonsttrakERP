import { Form, Button, InputGroup } from "react-bootstrap"
import { FaTrash } from "react-icons/fa"
import PropTypes from "prop-types"

function PurchaseRequestItem({ allItems, addedItems, setAddedItems, index }) {
  function selectedItemChanged(e) {
    const selectedId = e.target.value

    if (selectedId === "-1") return

    const addedItemsCpy = [...addedItems]
    addedItemsCpy[index].id = selectedId
    addedItemsCpy[index].load()

    setAddedItems(addedItemsCpy)
  }

  function valueChanged(e) {
    const addedItemsCpy = [...addedItems]
    addedItemsCpy[index][e.target.name] = e.target.value
    setAddedItems(addedItemsCpy)
  }

  function deleteItem() {
    const addedItemsCpy = [...addedItems]
    addedItemsCpy.splice(index, 1)
    setAddedItems(addedItemsCpy)
  }

  return (
    <div className="shadow pt-3 px-4 mb-4 rounded">
      <div className=" row justify-content-between">
        <div className="col-auto">
          <h1 className="display-6 fs-4">Item {index + 1}</h1>
        </div>
        <div className="col-auto">
          {!(index === 0 && addedItems.length === 1) && (
            <Button className="px-4" variant="danger" onClick={deleteItem}>
              <FaTrash className="me-2 mb-1" />
              Remove Item
            </Button>
          )}
        </div>
      </div>

      <div className="row">
        <div className="col">
          <Form.Group className="mb-3">
            <Form.Label>Item</Form.Label>
            <Form.Select
              value={addedItems[index].id}
              onChange={selectedItemChanged}
            >
              <option value="-1">Select Item</option>
              {allItems.map((item) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </Form.Select>
          </Form.Group>
        </div>
        <div className="col">
        <div className="row">
          <div className="col">
            <Form.Group className="mb-3">
              <Form.Label>Quantity</Form.Label>
              <InputGroup className="mb-3">
                <Form.Control
                  type="number"
                  name="qty"
                  value={addedItems[index].qty}
                  onChange={valueChanged}
                />
              </InputGroup>
            </Form.Group>
          </div>
          <div className="col">
            <Form.Group className="mb-3">
              <Form.Label>Measured In</Form.Label>
              <Form.Select
                name="unit"
                placeholder=" measurement unit"
                value={addedItems[index].unit}
                onChange={valueChanged}
              >
                {addedItems[index].units.map((unit, index) => (
                  <option key={index} value={index}>
                    {unit}
                  </option>
                ))}
              </Form.Select>
            </Form.Group>
          </div>
          </div>
        </div>
      </div>

      <div className="row">
        <div className="col">
          <Form.Group className="mb-3">
            <Form.Label>Specification</Form.Label>
            <Form.Control
              as="textarea"
              name="spec"
              rows={5}
              value={addedItems[index].spec}
              onChange={valueChanged}
            />
          </Form.Group>
        </div>
        <div className="col">
          <div className="row">
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Unit Cost</Form.Label>
                <Form.Control
                  type="number"
                  name="cost"
                  value={addedItems[index].cost}
                  onChange={valueChanged}
                />
              </Form.Group>
            </div>
            <div className="col">
              <Form.Group className="mb-3">
                <Form.Label>Total Cost</Form.Label>
                <Form.Control
                  type="number"
                  name="total"
                  value={addedItems[index].total}
                  onChange={valueChanged}
                />
              </Form.Group>
            </div>
          </div>
          <Form.Group className="mb-3">
            <Form.Label>Remark</Form.Label>
            <Form.Control
              name="reqRemark"
              value={addedItems[index].reqRemark}
              onChange={valueChanged}
            />
          </Form.Group>
        </div>
      </div>
    </div>
  )
}

PurchaseRequestItem.propTypes = {
  allItems: PropTypes.array.isRequired,
  addedItems: PropTypes.array.isRequired,
  setAddedItems: PropTypes.array.isRequired,
  index: PropTypes.number.isRequired,
}

export default PurchaseRequestItem
