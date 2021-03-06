// Generated by TinyPG v1.3 available at www.codeproject.com

using System;
using System.Collections.Generic;

namespace TinyPG
{
    #region Parser

    public partial class Parser 
    {
        private Scanner scanner;
        private ParseTree tree;
        
        public Parser(Scanner scanner)
        {
            this.scanner = scanner;
        }

        public ParseTree Parse(string input)
        {
            tree = new ParseTree();
            return Parse(input, tree);
        }

        public ParseTree Parse(string input, ParseTree tree)
        {
            scanner.Init(input);

            this.tree = tree;
            ParseStart(tree);
            tree.Skipped = scanner.Skipped;

            return tree;
        }

        private void ParseStart(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.Start), "Start");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.REPEAT);
            while (tok.Type == TokenType.REPEAT)
            {
                ParseCommandExpr(node);
            tok = scanner.LookAhead(TokenType.REPEAT);
            }

            
            tok = scanner.Scan(TokenType.EOF);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.EOF) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EOF.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseCodeBlock(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CodeBlock), "CodeBlock");
            parent.Nodes.Add(node);

            tok = scanner.LookAhead(TokenType.REPEAT);
            while (tok.Type == TokenType.REPEAT)
            {
                ParseCommandExpr(node);
            tok = scanner.LookAhead(TokenType.REPEAT);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseCommandLine(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CommandLine), "CommandLine");
            parent.Nodes.Add(node);

            ParseCommandExpr(node);

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseString(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.String), "String");
            parent.Nodes.Add(node);


            
            tok = scanner.Scan(TokenType.QUOTEBEGIN);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.QUOTEBEGIN) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.QUOTEBEGIN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            
            tok = scanner.LookAhead(TokenType.QUOTED, TokenType.NULL);
            switch (tok.Type)
            {
                case TokenType.QUOTED:
                    tok = scanner.Scan(TokenType.QUOTED);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.QUOTED) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.QUOTED.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.NULL:
                    tok = scanner.Scan(TokenType.NULL);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.NULL) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NULL.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            
            tok = scanner.Scan(TokenType.QUOTEEND);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.QUOTEEND) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.QUOTEEND.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseCommandExpr(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CommandExpr), "CommandExpr");
            parent.Nodes.Add(node);


            
            ParseRepeatExpr(node);

            
            tok = scanner.LookAhead(TokenType.NEWLINE, TokenType.WHITESPACE, TokenType.RETURN, TokenType.EMPTY);
            switch (tok.Type)
            {
                case TokenType.NEWLINE:
                    tok = scanner.Scan(TokenType.NEWLINE);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.NEWLINE) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NEWLINE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.WHITESPACE:
                    tok = scanner.Scan(TokenType.WHITESPACE);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.WHITESPACE) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.WHITESPACE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.RETURN:
                    tok = scanner.Scan(TokenType.RETURN);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.RETURN) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.RETURN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.EMPTY:
                    tok = scanner.Scan(TokenType.EMPTY);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.EMPTY) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EMPTY.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseRepeatExpr(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.RepeatExpr), "RepeatExpr");
            parent.Nodes.Add(node);


            
            tok = scanner.Scan(TokenType.REPEAT);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.REPEAT) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.REPEAT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            
            ParseCondExpr(node);

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseJoinCondition(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.JoinCondition), "JoinCondition");
            parent.Nodes.Add(node);

            tok = scanner.LookAhead(TokenType.AND, TokenType.OR);
            switch (tok.Type)
            {
                case TokenType.AND:
                    tok = scanner.Scan(TokenType.AND);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.AND) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.AND.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.OR:
                    tok = scanner.Scan(TokenType.OR);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.OR) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.OR.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseEqualitySymbol(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.EqualitySymbol), "EqualitySymbol");
            parent.Nodes.Add(node);

            tok = scanner.LookAhead(TokenType.EQUALITY, TokenType.NOTEQUAL, TokenType.LESSTHAN);
            switch (tok.Type)
            {
                case TokenType.EQUALITY:
                    tok = scanner.Scan(TokenType.EQUALITY);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.EQUALITY) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.EQUALITY.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.NOTEQUAL:
                    tok = scanner.Scan(TokenType.NOTEQUAL);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.NOTEQUAL) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NOTEQUAL.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.LESSTHAN:
                    tok = scanner.Scan(TokenType.LESSTHAN);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.LESSTHAN) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.LESSTHAN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseValueExpres(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ValueExpres), "ValueExpres");
            parent.Nodes.Add(node);


            
            tok = scanner.Scan(TokenType.OBJECT);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.OBJECT) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.OBJECT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            
            tok = scanner.LookAhead(TokenType.DOT);
            while (tok.Type == TokenType.DOT)
            {

                
                tok = scanner.Scan(TokenType.DOT);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.DOT) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.DOT.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }

                
                tok = scanner.Scan(TokenType.LENGTH);
                n = node.CreateNode(tok, tok.ToString() );
                node.Token.UpdateRange(tok);
                node.Nodes.Add(n);
                if (tok.Type != TokenType.LENGTH) {
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.LENGTH.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                    return;
                }
            tok = scanner.LookAhead(TokenType.DOT);
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseConditionComparer(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ConditionComparer), "ConditionComparer");
            parent.Nodes.Add(node);


            
            tok = scanner.LookAhead(TokenType.VARIABLE, TokenType.OBJECT);
            switch (tok.Type)
            {
                case TokenType.VARIABLE:
                    tok = scanner.Scan(TokenType.VARIABLE);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.VARIABLE) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.VARIABLE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.OBJECT:
                    ParseValueExpres(node);
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            
            ParseEqualitySymbol(node);

            
            tok = scanner.LookAhead(TokenType.QUOTEBEGIN, TokenType.NUMBER, TokenType.VARIABLE, TokenType.OBJECT);
            switch (tok.Type)
            {
                case TokenType.QUOTEBEGIN:
                    ParseString(node);
                    break;
                case TokenType.NUMBER:
                    tok = scanner.Scan(TokenType.NUMBER);
                    n = node.CreateNode(tok, tok.ToString() );
                    node.Token.UpdateRange(tok);
                    node.Nodes.Add(n);
                    if (tok.Type != TokenType.NUMBER) {
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.NUMBER.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                        return;
                    }
                    break;
                case TokenType.VARIABLE:
                case TokenType.OBJECT:
                    tok = scanner.LookAhead(TokenType.VARIABLE, TokenType.OBJECT);
                    switch (tok.Type)
                    {
                        case TokenType.VARIABLE:
                            tok = scanner.Scan(TokenType.VARIABLE);
                            n = node.CreateNode(tok, tok.ToString() );
                            node.Token.UpdateRange(tok);
                            node.Nodes.Add(n);
                            if (tok.Type != TokenType.VARIABLE) {
                                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.VARIABLE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                                return;
                            }
                            break;
                        case TokenType.OBJECT:
                            ParseValueExpres(node);
                            break;
                        default:
                            tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                            break;
                    }
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseConditionBracketed(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.ConditionBracketed), "ConditionBracketed");
            parent.Nodes.Add(node);


            
            tok = scanner.Scan(TokenType.BROPEN);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.BROPEN) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BROPEN.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            
            ParseConditionComparer(node);

            
            tok = scanner.LookAhead(TokenType.AND, TokenType.OR);
            if (tok.Type == TokenType.AND
                || tok.Type == TokenType.OR)
            {

                
                ParseJoinCondition(node);

                
                ParseConditionComparer(node);
            }

            
            tok = scanner.Scan(TokenType.BRCLOSE);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.BRCLOSE) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.BRCLOSE.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            parent.Token.UpdateRange(node.Token);
        }

        private void ParseCondExpr(ParseNode parent)
        {
            Token tok;
            ParseNode n;
            ParseNode node = parent.CreateNode(scanner.GetToken(TokenType.CondExpr), "CondExpr");
            parent.Nodes.Add(node);


            
            tok = scanner.Scan(TokenType.IF);
            n = node.CreateNode(tok, tok.ToString() );
            node.Token.UpdateRange(tok);
            node.Nodes.Add(n);
            if (tok.Type != TokenType.IF) {
                tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found. Expected " + TokenType.IF.ToString(), 0x1001, 0, tok.StartPos, tok.StartPos, tok.Length));
                return;
            }

            
            tok = scanner.LookAhead(TokenType.VARIABLE, TokenType.OBJECT, TokenType.BROPEN);
            switch (tok.Type)
            {
                case TokenType.VARIABLE:
                case TokenType.OBJECT:
                    ParseConditionComparer(node);
                    break;
                case TokenType.BROPEN:
                    ParseConditionBracketed(node);
                    break;
                default:
                    tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                    break;
            }

            
            tok = scanner.LookAhead(TokenType.AND, TokenType.OR);
            while (tok.Type == TokenType.AND
                || tok.Type == TokenType.OR)
            {

                
                ParseJoinCondition(node);

                
                tok = scanner.LookAhead(TokenType.VARIABLE, TokenType.OBJECT, TokenType.BROPEN);
                switch (tok.Type)
                {
                    case TokenType.VARIABLE:
                    case TokenType.OBJECT:
                        ParseConditionComparer(node);
                        break;
                    case TokenType.BROPEN:
                        ParseConditionBracketed(node);
                        break;
                    default:
                        tree.Errors.Add(new ParseError("Unexpected token '" + tok.Text.Replace("\n", "") + "' found.", 0x0002, 0, tok.StartPos, tok.StartPos, tok.Length));
                        break;
                }
            tok = scanner.LookAhead(TokenType.AND, TokenType.OR);
            }

            parent.Token.UpdateRange(node.Token);
        }


    }

    #endregion Parser
}
