using UnityEngine;
using System.Collections;

public class thanks : MonoBehaviour {

    public TextMesh g_text_partners;
    public TextMesh g_text_web;
    public TextMesh g_text_l_top;
    public TextMesh g_text_m_top;
    public TextMesh g_text_r_top;
    public TextMesh g_text_l;
    public TextMesh g_text_m;
    public TextMesh g_text_r;

     void Start () 
     {
         g_text_partners.text = "Partners \n \n" +
        "Haute école Arc \n" +
        "Sensorica \n" +
        "CHUV \n" +
        "CHUM Sainte Justine \n" +
        "Hôpital Necker \n" +
        "Fibrose kystique Québec \n" +
        "Concordia University \n" +
        "Université de Montréal \n" +
        "Canadian Academy \n" +
        " for the Knowledge Economy \n" +
        "Fondation praneo \n" +
        "Fondation Defitech \n" +
        "Mikorizal Software \n" +
        "IAV Engineering";

         g_text_web.text = "www.breathinggames.net";

         g_text_l_top.text = "Active contributors";
         g_text_m_top.text = " ";
         g_text_r_top.text = "Former contributors";

         g_text_l.text = "Jim Anastassiou \n" +
        "David Arango \n" +
        "Fabio Balli \n" +
        "Yves Berthiaume \n" +
        "Elise Boulay \n" +
        "Daniel Brastaviceanu \n" +
        "Tiberius Brastaviceanu \n" +
        "Annie Brochu \n" +
        "Dominique Correia \n" +
        "Thomas Daguenel \n" +
        "John Danger \n" +
        "Quentin de Halleux \n" +
        "François-Xavier Dupas \n" +
        "Hafen Gaudenz \n" +
        "Yannick Gervais \n" +
        "Stéphane Gobron \n" +
        "Valentin Gomez \n" +
        "David Grunenwald \n" +
        "Gérald Huguenin \n" +
        "Calin Ionescu";

         g_text_m.text = "Fabien Jeanneret \n" +
        "Jacques-Edouard Marcotte \n" +
        "Tammy Meyer \n" +
        "Camille Morasse \n" +
        "Thanh Nguyen \n" +
        "Laurent Pouget \n" +
        "Christian Voirol \n" +
        "Nicolas Wenk";

         g_text_r.text = "Ahmed Akl \n" +
        "Annick Bedard \n" +
        "Kim Berthiaume \n" +
        "Jérémy Bouchard \n" +
        "Pierre Philippe Brûlé \n" +
        "Wendy Chung \n" +
        "Sophie Courchesne \n" +
        "Jonathan Dextraze \n" +
        "Nicolas Dextraze \n" +
        "Nicolas Doduik \n" +
        "David Duguay \n" +
        "Kadeem Dunn \n" +
        "Damien Galan \n" +
        "Stéphane Geiser \n" +
        "Jérémy Méjane \n" +
        "Mark Melnykowycz \n" +
        "Florian Moncomble \n" +
        "Pascal Nataf \n" +
        "Claire Reierson \n" +
        "Patricia Sigam \n" +
        "Cyriaque Skrapits \n" +
        "Justine Sun \n" +
        "Mark Thompson";
     }
} 