using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Domains.Character
{
    public class CharacterAttribute
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public Attribute Attribute { get; set; }
        public AttributeType Type { get; set; }
    }

    //https://www.daemon.com.br/home/atributos-em-rpg/

    //Habilidades, Poderes, Magias, Rituais e Perícias

    public enum AttributeType
    {
        Physical,
        Mental,
        Spirit
    }

    //Os atributos estão classificados com adjetivos associados a números.
    //Uma pessoa comum(um homem/mulher adulto, da raça humana) possui TODOS os Atributos em “Normal”; 
    //uma pessoa que tenha força acima da média(um atleta, por exemplo) pode ter uma força “Boa” ou “Ótima”; 
    //um profissional conseguiria ter atributos “Incríveis” ou “Superiores” e heróis possuem Atributos “Heróicos”. 
    //Explicaremos melhor o que estes valores significam mais adiante.

    public enum Attribute
    {
        Strength,       //Determina a força física do Personagem, sua capacidade muscular. 
                        //um lutador magrinho de kung fu pode ser forte o bastante para quebrar pilhas de tijolos, 
                        //mas um fisiculturista musculoso dificilmente poderia igualar a proeza.
                        //A Força afeta o Bônus de Dano (dano extra) que ele é capaz de causar com armas de combate corporal 
                        //e o Peso Máximo que o Herói pode carregar ou sustentar (por poucos instantes). 
                        //Também determina o Alcance Máximo de objetos arremessados pelo Herói (quanto mais forte, mais longe ele é capaz de arremessar objetos, savvy?).
        
        Constitution,   //Determina o vigor, saúde e condição física do Personagem. De modo geral, 
                        //um Personagem com um baixo valor em Constituição é franzino, enquanto um valor alto garante uma saúde vigorosa.
                        //Isso não significa necessariamente que o Personagem seja forte ou fraco; isso é determinado pela Força.
                        //A Constituição determina a quantidade de Pontos de Vida — quanto mais alta a CON, mais PVs o Personagem terá (mais golpes ele aguentará).
                        //A CON também serve para qualquer Teste de Resistência que envolva capacidades físicas específicas (como por exemplo, prender a respiração, 
                        //agüentar a dor, resistir a venenos, corrosivos, alucinógenos e afins).
        
        Dexterity,      //Define a capacidade manual do Personagem, sua acuidade com as mãos e/ou pés. Não inclui a agilidade corporal, apenas a destreza manual. 
                        //Um Personagem com alta Destreza pode lidar melhor com armas, usar ferramentas, operar instrumentos delicados, atirar com arco-e-flecha, 
                        //agarrar objetos em pleno ar…
                        //Agilidade e Destreza também não estão necessariamente relacionados. Um Personagem pode ser um exímio artista, ou um grande arqueiro, mas completamente desastrado.
        
        Agility,        //Ao contrário da Destreza, a Agilidade não é válida para coisas feitas com as mãos, mas sim para o corpo todo. 
                        //Com um alto valor em Agilidade um Personagem pode se desviar melhor dos ataques de oponentes, equilibrar-se melhor sobre um muro, dançar com mais graça, 
                        //agarrar-se em parapeitos ou escapar de armadilhas… É importante fixar a diferença entre Destreza e Agilidade para fins de jogo.
                        //A Agilidade determina parte do Bônus de Defesa do Personagem, junto com a Esquiva ou o Escudo; é usada em Testes de Agilidade 
                        //(qualquer Teste que envolva equilíbrio, escapar de desmoronamentos, amortecer dano de quedas, agarrar-se em parapeitos ou canos, 
                        //saltar de uma carroça em movimento antes que ela caia em um penhasco, etc.).
        
        Willpower,      //Esta é a capacidade de concentração e determinação do Personagem. Uma alta Força de Vontade fará com que um Personagem resista a coisas como tortura psicológica, 
                        //hipnose, pânico, tentações e controle da mente. O Mestre também pode exigir Testes de Força de Vontade para verificar se um Personagem não fica apavorado 
                        //diante de uma situação amedrontadora. Também está relacionada com a Magia e poderes psíquicos.
                        //Usamos o termo WILL (originário do inglês Willpower) como abreviação para que os Jogadores não confundam com o Atributo Físico Força (FR). 
                        //Testes de Força de Vontade estão ligados diretamente com Testes de Resistência à Magia e Psiônicos, bem como Testes contra Dominação e Controle de Mentes.
        
        Inteligence,    //Inteligência é a capacidade de resolver problemas, nem mais e nem menos. Um Personagem inteligente está mais apto a compreender o que ocorre à sua volta 
                        //e não se deixa enganar tão facilmente. Também lida com a memória, capacidade de abstrair conceitos e descobrir coisas novas.
                        //Inteligência mexe com a Duração de Rituais e Poderes, afeta a Dificuldade em resistir a Poderes Psiônicos e também concede Pontos de Magia extras se o 
                        //Personagem for um Místico.
                        //Testes de Inteligência podem ser feitos para resolver enigmas, charadas, desafios matemáticos ou problemas de lógica.
        
        Wisdom,         //É a capacidade de observar o mundo à volta e perceber detalhes importantes — como aquela ponta de adaga aparecendo na curva do corredor. 
                        //Um Personagem com alta Percepção está sempre atento a coisas estranhas e minúcias quase imperceptíveis, enquanto o sujeito com Percepção baixa é distraído e avoado.
                        //É utilizado para fazer Testes para Encontrar Objetos Perdidos ou escondidos, Detectar Armadilhas, perceber inimigos ou passagens secretas.
        
        Charisma,        //Determina o charme do Personagem, sua capacidade de fazer com que outras pessoas gostem dele.
                        //Um alto valor em Carisma não quer dizer que ele seja bonito ou coisa assim, apenas simpático: uma modelo profissional que tenha alta Constituição e 
                        //baixo Carisma seria uma chata insuportável; um baixinho feio e mirrado, mas simpático, poderia reunir sem problemas montes de amigos à sua volta.
                        //Carisma também determina a Sorte do Personagem. O Bônus de Carisma indica quantas vezes por sessão de jogo o jogador pode Rolar uma Segunda Vez 
                        //os dados de um Teste ou Ataque realizado.
                        //O bônus de Carisma também é utilizado em Testes para conseguir descontos em negociações ou compras de equipamentos, para adquirir informações e em Testes de Reação 
                        //(utilizando este bônus em conjunto com a Perícia Manipulação-Sedução ou Manipulação-Diplomacia), para saber se um Coadjuvante gostou do seu Personagem ou não.
                        //Também é muito usado em conjurações e negociações com criaturas extraplanares (gênios, demônios, anjos, elementais e outros espíritos conjurados). 
                        //Um conjurador com Carisma baixo pode até mesmo ser atacado pelos seres que pretendia comandar…
        
        Soul            //Determina a extensão da sua alma, que lhe permite executar e aprender skills.
    }

    //TEN - vestir o manto
    //ZETSU - cortar, esconder, suprimir, descontinuar
    //REN - praticar, polir, refinar
    //HATSU - emitir, descarregar

    //GYO - TEN + REN manipular para uma área especifica.
    //IN - ZETSU + TEN Esconder
    //EN - TEN área ao redor
    //SHU - TEN + REN + HATSU revestir em um objeto
    //KO - GYO + TEN + ZETSU + REN manipular a aura
    //KEN - REN + TEN Fortificar/Intensificar um aparte do corpo
    //RYU - TEN + GYO - manimupalar percentual na distribuição.

    //fortificado (tanker, dano puro), transmutador (alterar propriedades, não transforma na propriedade apenas adquire a propriedade), 
    //conjurador (criar um material), especialista (curingas), manipulador (manioular objetos e criaturas), emissor (ataque a distancia)
}
