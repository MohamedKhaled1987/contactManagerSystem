#r "C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\9.0.0\System.Windows.Forms.dll"

open System
open System.Windows.Forms



open System
open System.Windows.Forms
open System.Collections.Generic
open System.Text.RegularExpressions
open System.ComponentModel

type Contact = { Name: string; PhoneNumber: string; Email: string }

let isValidName (name: string) =
    not (Regex.IsMatch(name, @"\d"))

let isValidPhoneNumber (phone: string) =
    Regex.IsMatch(phone, @"^\d+$")

let isValidEmail (email: string) =
    Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")

let form = new Form(Text = "Contact Manager System", Width = 800, Height = 610)

let lblName = new Label(Text = "Name:", Top = 20, Left = 20)
let txtName = new TextBox(Top = 20, Left = 200, Width = 300)

let lblPhone = new Label(Text = "Phone:", Top = 60, Left = 20)
let txtPhone = new TextBox(Top = 60, Left = 200, Width = 300)

let lblEmail = new Label(Text = "Email:", Top = 100, Left = 20)
let txtEmail = new TextBox(Top = 100, Left = 200, Width = 300)

let btnAdd = new Button(Text = "Add Contact", Top = 140, Left = 20, Width = 100)
let btnSearchByName = new Button(Text = "Search by Name", Top = 140, Left = 130, Width = 120)
let btnSearchByPhone = new Button(Text = "Search by Phone", Top = 140, Left = 260, Width = 110)
let btnUpdate = new Button(Text = "Update Contact", Top = 140, Left = 380, Width = 130)
let btnDelete = new Button(Text = "Delete Contact", Top = 140, Left = 520, Width = 110)
let btnDisplayAll = new Button(Text = "Display All Contacts", Top = 140, Left = 640, Width = 130)

let dataGridViewContacts = new DataGridView(Top = 200, Left = 20, Width = 750, Height = 350)

let mutable contacts = Map.empty<string, Contact>

let addContact name phone email =
    if not (isValidName name) then
        MessageBox.Show("Invalid name.") |> ignore
    elif not (isValidPhoneNumber phone) then
        MessageBox.Show("Invalid phone number.") |> ignore
    elif not (isValidEmail email) then
        MessageBox.Show("Invalid email.") |> ignore
    elif contacts.ContainsKey phone then
        MessageBox.Show("Contact already exists.") |> ignore
    else
        contacts <- contacts.Add(phone, { Name = name; PhoneNumber = phone; Email = email })
        MessageBox.Show("Contact added successfully!") |> ignore

let searchContactByName (contacts: Map<string, Contact>) (name: string) =
    contacts
    |> Map.toSeq
    |> Seq.filter (fun (_, contact) -> contact.Name.ToLower().Contains(name.ToLower()))
    |> Seq.map snd
    |> List.ofSeq

let searchByPhone phone =
    match contacts.TryFind phone with
    | Some contact -> [contact]
    | None -> []

let updateContact phone newName newPhone newEmail =
    match contacts.TryFind phone with
    | Some _ -> 
        contacts <- contacts.Remove(phone)
        addContact newName newPhone newEmail
    | None -> MessageBox.Show("Contact not found.") |> ignore

let deleteContact phone =
    if contacts.ContainsKey phone then
        contacts <- contacts.Remove(phone)
        MessageBox.Show("Contact deleted successfully!") |> ignore
    else
        MessageBox.Show("Contact not found.") |> ignore

let displayContacts (contactList: Contact list) =
    let bindingList = new BindingList<Contact>(contactList |> List.toArray)
    dataGridViewContacts.DataSource <- bindingList

let displayAllContacts () =
    let allContacts =  contacts |> Map.toSeq |> Seq.map snd |> List.ofSeq
    displayContacts allContacts

btnAdd.Click.Add(fun _ -> addContact txtName.Text txtPhone.Text txtEmail.Text)
btnSearchByName.Click.Add(fun _ -> searchContactByName contacts txtName.Text |> displayContacts)
btnSearchByPhone.Click.Add(fun _ -> searchByPhone txtPhone.Text |> displayContacts)
btnUpdate.Click.Add(fun _ -> updateContact txtPhone.Text txtName.Text txtPhone.Text txtEmail.Text)
btnDelete.Click.Add(fun _ -> deleteContact txtPhone.Text)
btnDisplayAll.Click.Add(fun _ -> displayAllContacts())

form.Controls.Add(lblName)
form.Controls.Add(txtName)
form.Controls.Add(lblPhone)
form.Controls.Add(txtPhone)
form.Controls.Add(lblEmail)
form.Controls.Add(txtEmail)
form.Controls.Add(btnAdd)
form.Controls.Add(btnSearchByName)
form.Controls.Add(btnSearchByPhone)
form.Controls.Add(btnUpdate)
form.Controls.Add(btnDelete)
form.Controls.Add(btnDisplayAll)
form.Controls.Add(dataGridViewContacts)

let startApp () =
    Application.Run(form)

let main _ =
    startApp ()
    0

startApp()