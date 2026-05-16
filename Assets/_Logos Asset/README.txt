LOGO SEQUENCE SYSTEM - UNITY

---

## DESCRIÇÃO

Sistema para exibir logos em sequência (splash screen).

Cada logo pode ter seu próprio comportamento (fade, tempo, etc),
enquanto um gerenciador controla a ordem e a troca entre elas.

---

## ESTRUTURA

LogoManager

* Controla a sequência de logos
* Ativa e desativa os objetos
* Carrega a próxima cena ao final

LogoBase

* Classe base para todas as logos
* Define duração e execução padrão

LogoFade (exemplo)

* Implementa efeito de fade (in / out)

---

## COMO USAR

1. Criar o Manager

* Crie um GameObject vazio
* Adicione o script: LogoManager

2. Criar as Logos

Para cada logo:

* Crie um objeto UI (Image)
* Adicione um script de efeito (ex: LogoFade)

IMPORTANTE:

* NÃO adicione LogoBase manualmente
* Use apenas scripts derivados (LogoFade, etc)

3. Configurar a sequência

* No LogoManager, preencha a lista "logos"
* Coloque na ordem desejada

4. Próxima cena

* Defina o nome da cena em "nextScene"
* A cena precisa estar em:
  File > Build Settings > Scenes In Build

---

## FUNCIONAMENTO

* Todas as logos começam desativadas
* O sistema ativa uma por vez
* Cada logo executa seu efeito
* Quando termina, a próxima é ativada
* No final, a cena é carregada

---

## CRIANDO NOVOS EFEITOS

Crie um novo script herdando de LogoBase:

* Controle animação
* Use coroutine
* Chame onFinish() no final

---

## REGRAS DO PROJETO

LOGOS OBRIGATÓRIAS (NÃO ALTERAR):

* Logo do projeto
* Logo da UNIFEI

LOGOS OPCIONAIS:

* Logo do projeto reimaginada
* Logo da equipe

Essas podem ser removidas ou alteradas.

