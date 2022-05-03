import Header from "../../layouts/Header";
import { Link } from "react-router-dom";
import UserGuideLayout from "./UserGuideLayout";
import Test from "./Test";

function UserGuide() {
    return (
        <>
            <Header title="User Guide" />

            <UserGuideLayout>
                <div title="Child 1">
                    Lorem ipsum dolor sit amet consectetur, adipisicing elit. Porro consectetur
                    tempore est illo ex deserunt nobis sunt consequatur saepe quis tempora
                    quibusdam, rerum doloribus, explicabo enim dolor quia! Distinctio, at. Neque
                    ducimus modi quos commodi ratione facere. Eaque tenetur totam voluptatem debitis
                    laudantium sequi nulla, optio quibusdam error, nostrum ea suscipit autem quod
                    illo enim aliquid vitae architecto non magni? Molestias error fuga suscipit!
                    Placeat beatae possimus aspernatur iusto odit eius cumque hic sunt aperiam, rem
                    adipisci in eligendi exercitationem saepe ipsam inventore dolores,
                    necessitatibus facere recusandae quae modi rerum. Tenetur beatae, reiciendis
                    obcaecati natus possimus neque maiores deserunt voluptate. Veniam cum,
                    reprehenderit aspernatur totam officiis dolores, harum doloribus consectetur,
                    optio voluptates aperiam error? Ad libero unde voluptatem asperiores deserunt.
                    Voluptatibus odio consequatur ab animi debitis totam cupiditate temporibus ea
                    illo hic magnam, harum facilis consequuntur, error, autem aspernatur! Minus
                    nulla nam architecto, error repellendus fuga ex! Voluptatibus, odio nobis.
                </div>

                <div title="Child 2">
                    Lorem ipsum dolor sit amet consectetur, adipisicing elit. Porro consectetur
                    tempore est illo ex deserunt nobis sunt consequatur saepe quis tempora
                    quibusdam, rerum doloribus, explicabo enim dolor quia! Distinctio, at. Neque
                    ducimus modi quos commodi ratione facere. Eaque tenetur totam voluptatem debitis
                    laudantium sequi nulla, optio quibusdam error, nostrum ea suscipit autem quod
                    illo enim aliquid vitae architecto non magni? Molestias error fuga suscipit!
                    Placeat beatae possimus aspernatur iusto odit eius cumque hic sunt aperiam, rem
                    adipisci in eligendi exercitationem saepe ipsam inventore dolores,
                    necessitatibus facere recusandae quae modi rerum. Tenetur beatae, reiciendis
                    obcaecati natus possimus neque maiores deserunt voluptate. Veniam cum,
                    reprehenderit aspernatur totam officiis dolores, harum doloribus consectetur,
                    optio voluptates aperiam error? Ad libero unde voluptatem asperiores deserunt.
                    Voluptatibus odio consequatur ab animi debitis totam cupiditate temporibus ea
                    illo hic magnam, harum facilis consequuntur, error, autem aspernatur! Minus
                    nulla nam architecto, error repellendus fuga ex! Voluptatibus, odio nobis.
                </div>
                <div title="test">
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Exercitationem ducimus
                    et quaerat illum sed cumque voluptas eaque, debitis molestiae necessitatibus.
                    Deleniti nemo sapiente maiores magnam dolorum esse reiciendis a! Dolores.
                    Adipisci ad repellat non culpa sit quas beatae, provident iste commodi facere
                    dolores cumque vitae distinctio, itaque delectus ex iusto alias unde nisi magnam
                    tempore velit fuga consectetur? Ipsa, officia? Odit natus ex sit vitae veniam
                    distinctio vel iusto enim placeat fugit accusamus, eveniet a esse! Vitae
                    repellendus, quo necessitatibus reiciendis ab quidem obcaecati voluptatem
                    incidunt laboriosam odio consectetur rerum.
                </div>

                <div title="ABC">
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Exercitationem ducimus
                    et quaerat illum sed cumque voluptas eaque, debitis molestiae necessitatibus.
                    Deleniti nemo sapiente maiores magnam dolorum esse reiciendis a! Dolores.
                    Adipisci ad repellat non culpa sit quas beatae, provident iste commodi facere
                    dolores cumque vitae distinctio, itaque delectus ex iusto alias unde nisi magnam
                    tempore velit fuga consectetur? Ipsa, officia? Odit natus ex sit vitae veniam
                    distinctio vel iusto enim placeat fugit accusamus, eveniet a esse! Vitae
                    repellendus, quo necessitatibus reiciendis ab quidem obcaecati voluptatem
                    incidunt laboriosam odio consectetur rerum.
                </div>

                <div title="DEF">
                    Lorem ipsum dolor sit amet consectetur adipisicing elit. Exercitationem ducimus
                    et quaerat illum sed cumque voluptas eaque, debitis molestiae necessitatibus.
                    Deleniti nemo sapiente maiores magnam dolorum esse reiciendis a! Dolores.
                    Adipisci ad repellat non culpa sit quas beatae, provident iste commodi facere
                    dolores cumque vitae distinctio, itaque delectus ex iusto alias unde nisi magnam
                    tempore velit fuga consectetur? Ipsa, officia? Odit natus ex sit vitae veniam
                    distinctio vel iusto enim placeat fugit accusamus, eveniet a esse! Vitae
                    repellendus, quo necessitatibus reiciendis ab quidem obcaecati voluptatem
                    incidunt laboriosam odio consectetur rerum.
                </div>

                <Test title="Test" />
            </UserGuideLayout>
        </>
    );
}

export default UserGuide;
